import { Injectable } from '@angular/core';
import { BoardsService } from '../../boards/services/board.service';
import { ComponentService } from '../../components/services/component.service';
import { Observable, forkJoin, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

export interface SharedOrderData {
  boards: any[];       // You can replace 'any' with your Board model
  components: any[];   // Replace 'any' with Component model
}

@Injectable({
  providedIn: 'root'
})
export class SharedOrderDataService {
  constructor(
    private boardsService: BoardsService,
    private componentsService: ComponentService
  ) {}

  getBoardsAndComponents(): Observable<SharedOrderData> {
    return forkJoin({
      boards: this.boardsService.getBoards().pipe(catchError(() => of([]))),
      components: this.componentsService.getComponents().pipe(catchError(() => of([])))
    });
  }
}