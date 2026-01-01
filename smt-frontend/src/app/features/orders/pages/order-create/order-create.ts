import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators, FormsModule } from '@angular/forms';
import { OrdersService } from '../../services/orders.service';
import { MATERIAL_PROVIDERS } from '../../../../shared/material/material.providers';
import { AddComponentToBoard } from '../add-component-to-board/add-component-to-board';
import { AddBoardComponent } from '../add-board-to-order/add-board-to-order';
import { CreateOrder } from '../../../../shared/models/create-order.model';
import { OrderBoard } from '../../../../shared/models/order-board.model';
import { BoardComponent } from '../../../../shared/models/board-component.model';
import { forkJoin, switchMap } from 'rxjs';
import { BoardsService } from '../../../boards/services/board.service';

@Component({
  selector: 'app-create-order',
  standalone: true,
  imports: [CommonModule, 
  FormsModule,
  MATERIAL_PROVIDERS,
  AddComponentToBoard,
  AddBoardComponent
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
 form:any;

   constructor(
    private fb: FormBuilder,
    private ordersService: OrdersService,
    private boardService: BoardsService
  ) {}
 
 

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

    // Step 1: Create the order
    this.ordersService.createOrder({
      name: this.order.name,
      description: this.order.description,
      orderDate: this.order.orderDate
    }).subscribe({
      next: (createdOrder) => {
        const orderId = createdOrder.id;

        // Step 2: Add boards sequentially
        const boardRequests = this.order.boards.map(board =>
          this.ordersService.addBoardToOrder(orderId, board.boardId).pipe(
            // Step 3: Add components for this board
            switchMap(() => {
              const componentRequests = board.components.map(c =>
                this.boardService.addComponentToBoard(board.boardId, c.componentId, c.quantity)
              );
              return forkJoin(componentRequests);
            })
          )
        );

        // Execute all boards+components
        forkJoin(boardRequests).subscribe({
          next: () => {
            this.loading = false;
            this.createdOrderId = orderId;
            console.log('Order created successfully:', this.createdOrderId);
          },
          error: (err) => {
            this.loading = false;
            console.error('Error adding boards/components:', err);
          }
        });
      },
      error: (err) => {
        this.loading = false;
        console.error('Error creating order:', err);
      }
    });
  }
}