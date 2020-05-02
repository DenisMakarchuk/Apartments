import { Component, OnInit } from '@angular/core';
import { NgForm, FormControl, FormGroup, FormBuilder } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';

import { ApartmentSearchService } from 'src/app/core/nswag.generated.service';
import { CountryDTO } from 'src/app/core/nswag.generated.service';


import { SearchParameters } from 'src/app/core/nswag.generated.service';
import { ISearchParameters } from 'src/app/core/nswag.generated.service';

import { ApartmentSearchDTO } from 'src/app/core/nswag.generated.service';


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
    private formBuilder: FormBuilder,
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


  searchApartments(): void {
    this.searchService.getAllApartments(this.searchForm.value)
      .subscribe(apartments => this.apartments = apartments);
  }
}