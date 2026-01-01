import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddComponentToBoard } from './add-component-to-board';

describe('AddComponentToBoard', () => {
  let component: AddComponentToBoard;
  let fixture: ComponentFixture<AddComponentToBoard>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddComponentToBoard]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddComponentToBoard);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
