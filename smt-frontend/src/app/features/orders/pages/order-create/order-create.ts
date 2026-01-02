import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators, FormsModule } from '@angular/forms';
import { OrdersService } from '../../services/orders.service';
import { MATERIAL_PROVIDERS } from '../../../../shared/material/material.providers';
import { AddComponentToBoard } from '../add-component-to-board/add-component-to-board';
import { AddBoardToOrder } from '../add-board-to-order/add-board-to-order';
import { CreateOrder } from '../../../../shared/models/create-order.model';
import { OrderBoard } from '../../../../shared/models/order-board.model';
import { BoardComponent } from '../../../../shared/models/board-component.model';
import { forkJoin, map, switchMap } from 'rxjs';
import { BoardsService } from '../../../boards/services/board.service';

@Component({
  selector: 'app-create-order',
  standalone: true,
  imports: [CommonModule, 
  FormsModule,
  MATERIAL_PROVIDERS,
  AddComponentToBoard,
  AddBoardToOrder
],
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

      // STEP 1: Create Order
      this.ordersService.createOrder({
        name: this.order.name,
        description: this.order.description,
        orderDate: this.order.orderDate
      }).subscribe({
        next: (createdOrder) => {
          const orderId = createdOrder.id;

          // STEP 2: Add boards + components
          const boardRequests = this.order.boards.map(board =>
            this.ordersService.addBoardToOrder(orderId, board.boardId).pipe(
              switchMap(() => {
                const componentRequests = board.components.map(c =>
                  this.ordersService.addComponentToBoard(
                    orderId,
                    board.boardId,
                    c.componentId,
                    c.quantity
                  )
                );
                return forkJoin(componentRequests);
              })
            )
          );

          forkJoin(boardRequests).subscribe({
            next: () => {
              this.loading = false;
              this.createdOrderId = orderId;
              console.log('Order created successfully:', orderId);
            },
            error: (err) => {
              this.loading = false;
              console.error('Error adding boards/components', err);
            }
          });
        },
        error: (err) => {
          this.loading = false;
          console.error('Error creating order', err);
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