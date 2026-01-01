import { Injectable } from "@angular/core";
import { ApiService } from "../../../core/services/api.service";
import { Board } from "../../../shared/models/board.model";

@Injectable({
  providedIn: 'root'
})
export class BoardsService {
  constructor(private api: ApiService) {}

  addComponentToBoard(boardId: string,componentId: string,quantity: number
  ) {
    return this.api.post<void>(`boards/${boardId}/components`,{ componentId, quantity }
    );
  }

  getBoards() 
  {
    return this.api.get<Board[]>("boards");
  }
}