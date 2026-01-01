import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddBoardToOrder } from './add-board-to-order';

describe('AddBoardToOrder', () => {
  let component: AddBoardToOrder;
  let fixture: ComponentFixture<AddBoardToOrder>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddBoardToOrder]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddBoardToOrder);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
