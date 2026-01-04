import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OrderUpdate } from './order-update';

describe('OrderUpdate', () => {
  let component: OrderUpdate;
  let fixture: ComponentFixture<OrderUpdate>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [OrderUpdate]
    })
    .compileComponents();

    fixture = TestBed.createComponent(OrderUpdate);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
