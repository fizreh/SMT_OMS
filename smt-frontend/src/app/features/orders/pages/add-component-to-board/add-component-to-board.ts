import { CommonModule } from "@angular/common";
import { Component, EventEmitter, Input, Output } from "@angular/core";
import { FormBuilder, FormsModule, ReactiveFormsModule, Validators } from "@angular/forms";
import { BoardsService } from "../../../boards/services/board.service";
import { MATERIAL_PROVIDERS } from "../../../../shared/material/material.providers";
import { BoardComponent } from "../../../../shared/models/board-component.model";
import { ComponentService } from "../../../components/services/component.service";

@Component({
  selector: 'app-add-component',
  standalone: true,
  imports: [CommonModule, 
  FormsModule,
  MATERIAL_PROVIDERS],
  templateUrl: './add-component-to-board.html'
})
export class AddComponentToBoard {
   @Input() boardId!: string; // Board to which component will be added
   @Output() componentAdded = new EventEmitter<BoardComponent>();

  components: { id: string; name: string }[] = []; // list of components to populate select
  selectedComponentId: string = '';
  quantity: number = 1;

  constructor(private boardsService: BoardsService, private componentService: ComponentService) {}

   ngOnInit() {
    // Fetch components from backend
    this.componentService.getComponents().subscribe((c: any[]) => {
      this.components = c;
    });
  }

  addComponent() {
    if (!this.selectedComponentId || this.quantity <= 0) return;

     const selected = this.components.find(c => c.id === this.selectedComponentId);
    if (selected && this.quantity > 0) {

      const boardComponent: BoardComponent = {
        boardId:this.boardId,
        componentId:this.selectedComponentId,
        componentName: selected.name,
        quantity:this.quantity    
          };
      
      this.componentAdded.emit(boardComponent);


      this.selectedComponentId = '';
      this.quantity = 1; // Reset
    }
  }
  }
