import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';

import { ApartmentSearchService, OrderUserService } from 'src/app/services/nswag.generated.service';
import { LoggedService } from 'src/app/services/logged.service';
import { CountryDTO,
         ApartmentSearchDTO,
         AddressSearchDTO,
         CountrySearchDTO
} from 'src/app/services/nswag.generated.service';

import { PagedRequestOfSearchParameters, PagedResponseOfApartmentSearchView } from 'src/app/services/nswag.generated.service';
import { SearchParametersService } from 'src/app/services/search-parameters.service';

@Component({
  selector: 'app-search-view',
  templateUrl: './search-view.component.html',
  styleUrls: ['./search-view.component.scss']
})
export class SearchViewComponent implements OnInit {

  countries: CountryDTO[];
  pages: number[] = [];

  country: CountrySearchDTO;
  address: AddressSearchDTO;
  apartment: ApartmentSearchDTO;

  request: PagedRequestOfSearchParameters;
  response: PagedResponseOfApartmentSearchView;

  searchForm: FormGroup;
  requestForm: FormGroup;

  constructor(
    private searchService: ApartmentSearchService,
    private fb: FormBuilder,
    private authService: LoggedService,
    private avRoute: ActivatedRoute, 
    private router: Router,
    private postman: SearchParametersService
  ) {
    this.postman.request$
    .subscribe(request => this.request = request);

    this.postman.response$
    .subscribe(response => {
      this.response = response;

      this.pages = [];
      for (let index = 1; index <= response.totalPages; index++) 
        this.pages.push(index);
    });
  }

  ngOnInit(): void {
  }

  newPage(): void {
    this.searchService.getAllApartments(this.request)
      .subscribe(response => {
        this.response = response;

        this.pages = [];
        for (let index = 1; index <= response.totalPages; index++) {
          this.pages.push(index);

        this.postman.setSearchInfo(this.request, this.response);
        this.postman.getRequestInfo();
        this.postman.getResponseInfo();
        };
      });
  }

  get isLoggedIn(): boolean {
    return this.authService.isLoggedIn;
  }

  previousePage(){
    if (this.response != null && this.response.hasPreviousPage) {
      this.request.pageNumber -= 1;
      this.newPage();
    }
  }

  nextPage(){
    if (this.response != null && this.response.hasNextPage) {
      this.request.pageNumber += 1;
      this.newPage();
    }
  }

  getPage(page: number){
    if (this.response != null && page > 0 && page <= this.response.totalPages) {
      this.request.pageNumber = page;
      this.newPage();
    }
  }
}
