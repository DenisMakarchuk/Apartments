import { Component, OnInit } from '@angular/core';
import { NgForm, FormControl } from '@angular/forms';

import { ApartmentSearchService } from 'src/app/core/nswag.generated.service';
import { CountryDTO } from 'src/app/core/nswag.generated.service';

import { SearchParameters } from 'src/app/core/nswag.generated.service';
import { ApartmentSearchDTO } from 'src/app/core/nswag.generated.service';


@Component({
  selector: 'app-apartment-search',
  templateUrl: './apartment-search.component.html',
  styleUrls: ['./apartment-search.component.scss']
})
export class ApartmentSearchComponent implements OnInit {

  countries: CountryDTO[];
  params: SearchParameters;
  apartments: ApartmentSearchDTO[];
  dates: Date[];

  countryControl: FormControl;
  cityControl: FormControl;
  roomsFromControl: FormControl;
  roomsTillControl: FormControl;
  priceFromControl: FormControl;
  priceTillControl: FormControl;
  datesControl: FormControl;

  constructor(private searchService: ApartmentSearchService) { }

  ngOnInit(): void {
    this.getCountries();
        
    this.countryControl = new FormControl('');
    this.cityControl = new FormControl('');
    this.roomsFromControl = new FormControl();
    this.roomsTillControl = new FormControl();
    this.priceFromControl = new FormControl();
    this.priceTillControl = new FormControl();
    this.datesControl = new FormControl(this.dates);

    this.resetForm();
  }

  resetForm() {
    this.params = new SearchParameters();
    this.countryControl.valueChanges.subscribe(value => this.params.countryId = value);
    this.cityControl.valueChanges.subscribe(value => this.params.cityName = value);
    this.roomsFromControl.valueChanges.subscribe(value => this.params.roomsFrom = value);
    this.roomsTillControl.valueChanges.subscribe(value => this.params.roomsTill = value);
    this.priceFromControl.valueChanges.subscribe(value => this.params.priceFrom = value);
    this.priceTillControl.valueChanges.subscribe(value => this.params.priceTill = value);
    this.datesControl.valueChanges.subscribe(value => this.params.needDates = value);
  }

  getCountries(): void {
    this.searchService.getAllCountries()
      .subscribe(countries => this.countries = countries);
  }


  searchApartments(): void {
    this.searchService.getAllApartments(this.params)
      .subscribe(apartments => this.apartments = apartments);
  }

  
}