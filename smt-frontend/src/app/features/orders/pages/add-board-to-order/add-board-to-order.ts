import { CommonModule } from "@angular/common";
import { Component, EventEmitter, Input, Output } from "@angular/core";
import { FormBuilder, FormsModule, ReactiveFormsModule, Validators } from "@angular/forms";
import { OrdersService } from "../../services/orders.service";
import { MATERIAL_PROVIDERS } from "../../../../shared/material/material.providers";
import { OrderBoard } from "../../../../shared/models/order-board.model";
import { BoardsService } from "../../../boards/services/board.service";

@Component({
  selector: 'app-add-board-to-order',
  standalone: true,
  imports: [CommonModule, 
    FormsModule,
  MATERIAL_PROVIDERS],
  templateUrl: './add-board-to-order.html'
})
export class AddBoardComponent {
  @Input() boardId!: string; // Board to which component will be added
  boards: { id: string; name: string }[] = []; // boards to populate dropdown
  @Output() boardAdded = new EventEmitter<OrderBoard>(); // emit selected board to parent

  
  selectedBoardId: string = '';

  constructor(private boardsService: BoardsService) {}

  ngOnInit() {
    // Fetch boards from backend
    this.boardsService.getBoards().subscribe((b: any[]) => {
      this.boards = b;
      console.log("BOARDS: ",this.boards);
    });
  }

 addBoard() {
     if (!this.selectedBoardId) return;
 
     const board: OrderBoard = {
      boardId: this.selectedBoardId,
      boardName: this.boards.find(b => b.id === this.selectedBoardId)?.name || '',
      description: '',
      length: 0,
      width: 0,
      components: []
    };

    this.boardAdded.emit(board);

       // Reset selection
    this.selectedBoardId = '';
 
   }
}