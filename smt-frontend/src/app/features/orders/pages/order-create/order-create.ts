import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormsModule } from '@angular/forms';
import { OrdersService } from '../../services/orders.service';
import { MATERIAL_PROVIDERS } from '../../../../shared/material/material.providers';
import { AddComponentToBoard } from '../add-component-to-board/add-component-to-board';

import { CreateOrder } from '../../../../shared/models/create-order.model';
import { OrderBoard } from '../../../../shared/models/order-board.model';
import { BoardComponent } from '../../../../shared/models/board-component.model';
import { map} from 'rxjs';
import { BoardsService } from '../../../boards/services/board.service';
import { AddBoardToOrder } from "../add-board-to-order/add-board-to-order";
import { CreateOrderModel } from '../../../../shared/models/create-order-models/order-create.model';

@Component({
  selector: 'app-create-order',
  standalone: true,
  imports: [CommonModule,
    FormsModule,
    MATERIAL_PROVIDERS,
    AddComponentToBoard, AddBoardToOrder],
  templateUrl: './order-create.html'
})
export class CreateOrderComponent {
   order: CreateOrder = {
    name: '',
    description: '',
    orderDate: new Date().toISOString(),
    boards: []
  };
  loading = false;
  createdOrderId: string | null = null;
  orderExists = false;
  errorMessage = '';
 form:any;

   constructor(
    private fb: FormBuilder,
    private ordersService: OrdersService,
    private boardService: BoardsService
  ) {}

  private checkOrderExists(name: string) {
  return this.ordersService.getOrders().pipe(
    map(orders =>
      orders.some(
        o => o.name.trim().toLowerCase() === name.trim().toLowerCase()
      )
    )
  );
 }
 
 

    // Add a board with empty components
  addBoard(board: OrderBoard) {
    this.order.boards.push({
      boardId: board.boardId,
      boardName: board.boardName,
      description:board.description,
      length:board.length,
      width:board.width,
      components: []
    });
  }

   // Add a component to a specific board
  addComponentToBoard(boardId: string, component: BoardComponent) {
    const board = this.order.boards.find(b => b.boardId === boardId);
    if (board) {
      board.components.push(component);
    }
  }
  // Enable submit only if order name and boards/components exist
  canSubmit(): boolean {
    return (
      this.order.name.trim().length > 0 &&
      this.order.boards.length > 0 &&
      this.order.boards.every(b => b.components.length > 0)
    );
  }

  // Submit the order + boards + components
  submit() {
  if (!this.canSubmit()) return;

  this.loading = true;
  this.errorMessage = '';
  this.orderExists = false;

// STEP 0: Check if order already exists
  this.checkOrderExists(this.order.name).subscribe({
    next: (exists) => {
      if (exists) {
        this.loading = false;
        this.orderExists = true;
        this.errorMessage = 'An order with this name already exists.';
        return;
      }

      // STEP 1: Prepare payload according to backend DTO
  const order: CreateOrderModel = {
  name: this.order.name,
  description: this.order.description,
  orderDate: this.order.orderDate,
  boards: this.order.boards.map(b => ({
    boardId: b.boardId,
    components: b.components.map(c => ({
      componentId: c.componentId,
      quantity: c.quantity
    }))
  }))
};

console.log("ORDER:",order)

      // STEP 2: Call backend full-create API
      this.ordersService.createFullOrder(order).subscribe({
        next: (createdOrder) => {
          this.loading = false;
          this.createdOrderId = createdOrder.id;
          console.log('Order created successfully:', createdOrder.id);
        },
        error: (err) => {
          this.loading = false;
          console.error('Error creating full order', err);
          this.errorMessage = 'Failed to create order. See console for details.';
        }
      });
    },
    error: () => {
      this.loading = false;
      this.errorMessage = 'Unable to validate order uniqueness.';
    }
  });



  
}


}