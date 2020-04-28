import { Component, OnInit } from '@angular/core';

import { ApartmentSearchService } from 'src/app/core/nswag.generated.service';
import { CountryDTO } from 'src/app/core/nswag.generated.service';



@Component({
  selector: 'app-apartment-search',
  templateUrl: './apartment-search.component.html',
  styleUrls: ['./apartment-search.component.scss']
})
export class ApartmentSearchComponent implements OnInit {

  constructor(private searchService: ApartmentSearchService) { }

  ngOnInit(): void {
    this.getCountries();
  }

  countries: CountryDTO[];

  getCountries(): void {
    this.searchService.getAllCountries()
      .subscribe(countries => this.countries = countries);
  }
}
