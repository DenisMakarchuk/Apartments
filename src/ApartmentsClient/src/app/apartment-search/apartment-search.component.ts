import { Component, OnInit } from '@angular/core';
import { NgForm, FormControl, FormGroup, FormBuilder } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';

import { ApartmentSearchService, UserService, OrderUserService } from 'src/app/core/nswag.generated.service';
import { CountryDTO } from 'src/app/core/nswag.generated.service';


import { SearchParameters } from 'src/app/core/nswag.generated.service';
import { ISearchParameters } from 'src/app/core/nswag.generated.service';

import { ApartmentSearchDTO, AddOrder } from 'src/app/core/nswag.generated.service';


@Component({
  selector: 'app-apartment-search',
  templateUrl: './apartment-search.component.html',
  styleUrls: ['./apartment-search.component.scss']
})
export class ApartmentSearchComponent implements OnInit {

  countries: CountryDTO[];
  searchParams: SearchParameters;
  apartments: ApartmentSearchDTO[];
  data: ISearchParameters;

  addOrder: AddOrder;

  searchForm: FormGroup;

  countryId: string;
  cityName: string;
  roomsFrom: number;
  roomsTill: number;
  priceFrom: number;
  priceTill: number;
  dates: Date[];

  constructor(
    private searchService: ApartmentSearchService,
    private orderService: OrderUserService,
    private formBuilder: FormBuilder,
    private authService: UserService,
    private avRoute: ActivatedRoute, 
    private router: Router
    ) { 
      this.searchForm = this.formBuilder.group({
        countryId: '',
        cityName: '',
        roomsFrom: 0,
        roomsTill: 0,
        priceFrom: 0,
        priceTill: 0,
        dates: []
      })

    }

  ngOnInit(): void {
    this.getCountries();
  }

  getCountries(): void {
    this.searchService.getAllCountries()
      .subscribe(countries => this.countries = countries);
  }

  sendDates(){
    this.searchService.sendDates();
  }

  searchApartments(): void {
    console.log(this.searchForm.value.dates);
    this.searchService.getAllApartments(this.searchForm.value)
      .subscribe(apartments => this.apartments = apartments);
  }

  get isLoggedIn(): boolean {
    return this.authService.isLoggedIn;}
}