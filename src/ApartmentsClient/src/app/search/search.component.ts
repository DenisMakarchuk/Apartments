import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';

import { ApartmentSearchService, MakeDatesArrayHelperService } from 'src/app/services/nswag.generated.service';
import { CountryDTO } from 'src/app/services/nswag.generated.service';

import { PagedResponseOfApartmentSearchView } from 'src/app/services/nswag.generated.service';
import { SearchParametersService } from 'src/app/services/search-parameters.service';


@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.scss']
})
export class SearchComponent implements OnInit {

  countries: CountryDTO[];
  pages: number[] = [];

  response: PagedResponseOfApartmentSearchView;

  searchForm: FormGroup;
  requestForm: FormGroup;

  constructor(
    private searchService: ApartmentSearchService,
    private dateService: MakeDatesArrayHelperService,
    private fb: FormBuilder,
    private postman: SearchParametersService
    ) { 
      this.searchForm = this.fb.group({
        countryId: '',
        cityName: '',
        roomsFrom: null,
        roomsTill: null,
        priceFrom: null,
        priceTill: null,
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
    this.dateService.getDatesArray(this.requestForm.value.data.needDates === null ? [] : this.requestForm.value.data.needDates)
    .subscribe(dates => {
      this.requestForm.value.data.needDates = dates;

      this.searchService.getAllApartments(this.requestForm.value)
        .subscribe(response => {
          this.response = response;

          this.postman.setSearchInfo(this.requestForm.value, this.response);
          this.postman.getRequestInfo();
          this.postman.getResponseInfo();
        });
    });
  }
}