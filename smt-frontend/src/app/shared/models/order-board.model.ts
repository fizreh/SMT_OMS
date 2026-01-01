import { BoardComponent } from './board-component.model';

export interface OrderBoard {
  boardId: string;
  boardName?: string;  
  description:string;
  length: number;
  width: number;         // optional for UI
  components: BoardComponent[];
}