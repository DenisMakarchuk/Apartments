import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { PagedRequestOfSearchParameters, PagedResponseOfApartmentSearchView } from 'src/app/services/nswag.generated.service';

@Injectable({
  providedIn: 'root'
})
export class SearchParametersService {

  constructor() { }

  request: PagedRequestOfSearchParameters;
  response: PagedResponseOfApartmentSearchView;

  private requestSource = new Subject<PagedRequestOfSearchParameters>();
  private responseSource = new Subject<PagedResponseOfApartmentSearchView>();

  request$ = this.requestSource.asObservable();
  response$ = this.responseSource.asObservable();
    
  setSearchInfo(request: PagedRequestOfSearchParameters, response: PagedResponseOfApartmentSearchView){
    this.request = request;
    this.response = response;
  }

  getRequestInfo(){
    this.requestSource.next(this.request);
  }

  getResponseInfo(){
    this.responseSource.next(this.response);
  }
}
