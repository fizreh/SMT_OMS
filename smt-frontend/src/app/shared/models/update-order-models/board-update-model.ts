import { ComponentUpdateModel } from "./component-update.model";

export interface BoardUpdateModel {
  boardId: string;
  components: ComponentUpdateModel[];
}