import { BoardComponentModel } from "./board-component-create-model";

export interface BoardOrderModel {
  boardId: string;
  components: BoardComponentModel[];
}