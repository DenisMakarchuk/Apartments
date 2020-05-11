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
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.scss']
})
export class SearchComponent implements OnInit {

  countries: CountryDTO[];
  pages: number[] = [];

  country: CountrySearchDTO;
  address: AddressSearchDTO;
  apartment: ApartmentSearchDTO;

  request: PagedRequestOfSearchParameters = new PagedRequestOfSearchParameters();
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
      this.searchForm = this.fb.group({
        countryId: '',
        cityName: '',
        roomsFrom: 0,
        roomsTill: 0,
        priceFrom: 0,
        priceTill: 0,
        needDates: []
      })
      this.requestForm = this.fb.group({
        data: this.searchForm,
        pageNumber: 1,
        pageSize: 20
      })
    }

  ngOnInit(): void {
    this.getCountries();
  }

  getCountries(): void {
    this.searchService.getAllCountries()
      .subscribe(countries => this.countries = countries);
  }

  searchApartments(): void {
    this.searchService.getAllApartments(this.requestForm.value)
      .subscribe(response => {
        this.response = response;

        this.postman.setSearchInfo(this.requestForm.value, this.response);
        this.postman.getRequestInfo();
        this.postman.getResponseInfo();
      });
  }
}