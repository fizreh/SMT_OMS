import { Injectable } from "@angular/core";
import { ApiService } from "../../../core/services/api.service";
import { Board } from "../../../shared/models/board.model";

@Injectable({
  providedIn: 'root'
})
export class BoardsService {
  constructor(private api: ApiService) {}

  getBoards() 
  {
    return this.api.get<Board[]>("boards");
  }
}