import { Component, Input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators, FormArray } from '@angular/forms';
import { OrdersService } from '../../services/orders.service';
import { OrderUpdateModel } from '../../../../shared/models/update-order-models/order-update.model';
import { ActivatedRoute, Router } from '@angular/router';
import { MATERIAL_PROVIDERS } from '../../../../shared/material/material.providers';

@Component({
  selector: 'app-order-update',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule,
    MATERIAL_PROVIDERS
  ],
  templateUrl: './order-update.html'
})
export class OrderUpdateComponent implements OnInit {
  orderForm!: FormGroup;
  loading = false;
  orderId!: string;

  constructor(
    private fb: FormBuilder,
    private ordersService: OrdersService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.orderId = this.route.snapshot.paramMap.get('id')!;
    this.initForm();
    this.loadOrder();
  }


  initForm() {
    this.orderForm = this.fb.group({
      name: ['', Validators.required],
      description: [''],
      orderDate: ['', Validators.required],
      boards: this.fb.array([])
    });
  }


  get boards(): FormArray {
    return this.orderForm.get('boards') as FormArray;
  }

  components(boardIndex: number): FormArray {
    return this.boards.at(boardIndex).get('components') as FormArray;
  }

 
  createBoard(boardData?: any): FormGroup {
    return this.fb.group({
      boardId: [boardData?.boardId || '', Validators.required],
      name: [boardData?.name || '', Validators.required],
      description: [boardData?.description || ''],
      components: this.fb.array(
        boardData?.components?.map((c: any) => this.createComponent(c)) || []
      )
    });
  }

  createComponent(compData?: any): FormGroup {
    return this.fb.group({
      componentId: [compData?.componentId || '', Validators.required],
      name: [compData?.name || '', Validators.required],
      quantity: [compData?.quantity || 1, [Validators.required, Validators.min(1)]]
    });
  }

  
  loadOrder() {
    this.loading = true;
    this.ordersService.getOrderById(this.orderId).subscribe({
      next: (order: any) => {
        this.orderForm.patchValue({
          name: order.name,
          description: order.description,
          orderDate: order.orderDate
        });

        order.boards.forEach((b: any) => this.boards.push(this.createBoard(b)));
        this.loading = false;
      },
      error: () => {
        this.loading = false;
        console.error('Failed to load order');
      }
    });
  }

 
  addBoard() {
    this.boards.push(this.createBoard());
  }

  removeBoard(index: number) {
    this.boards.removeAt(index);
  }

  addComponent(boardIndex: number) {
    this.components(boardIndex).push(this.createComponent());
  }

  removeComponent(boardIndex: number, compIndex: number) {
    this.components(boardIndex).removeAt(compIndex);
  }

 
  submit() {
    if (this.orderForm.invalid) return;

    this.loading = true;

    const payload = this.orderForm.value;
    this.ordersService.updateOrder(payload).subscribe({
      next: () => {
        this.loading = false;
        alert('Order updated successfully!');
        this.router.navigate(['/orders']);
      },
      error: (err) => {
        this.loading = false;
        console.error('Failed to update order', err);
      }
    });
  }
}