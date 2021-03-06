import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class GetCommentsService {

  constructor() { }

  private goSource = new Subject<boolean>();

  go$ = this.goSource.asObservable();
    
  go(go: boolean){
    this.goSource.next(go);
  }
}
