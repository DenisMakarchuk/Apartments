import { Component, OnInit } from '@angular/core';

import { ApartmentView } from 'src/app/core/nswag.generated.service';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';

import { ApartmentUserService }  from 'src/app/core/nswag.generated.service';
import { ApartmentSearchService } from 'src/app/core/nswag.generated.service';

import { ApartmentDTO } from 'src/app/core/nswag.generated.service';
import { AddressDTO } from 'src/app/core/nswag.generated.service';
import { CountryDTO } from 'src/app/core/nswag.generated.service';


@Component({
  selector: 'app-apartment-detail',
  templateUrl: './apartment-detail.component.html',
  styleUrls: ['./apartment-detail.component.scss']
})
export class ApartmentDetailComponent implements OnInit {

  constructor(
    private route: ActivatedRoute,
    private apartmentService: ApartmentUserService,
    private location: Location,
    private searchService: ApartmentSearchService
  ) { }

  ngOnInit(): void {
    this.getCountries();
  }

  countries: CountryDTO[];
  apartmentView: ApartmentView;
  country: CountryDTO;
  address: AddressDTO;
  apartment: ApartmentDTO;

  getHero(): void {
    const id = this.route.snapshot.paramMap.get('id');
    this.apartmentService.getApartmentById(id)
      .subscribe(apartmentView => this.apartmentView = apartmentView);

      this.country = this.apartmentView?.country;
      this.address = this.apartmentView?.address;
      this.apartment = this.apartmentView?.apartment;
  }

  getCountries(): void {
    this.searchService.getAllCountries()
      .subscribe(countries => this.countries = countries);
  }

  goBack(): void {
    this.location.back();
  }

  save(): void {
    this.apartmentView.country = this.country;
    this.apartmentView.address = this.address;
    this.apartmentView.address.countryId = this.country.id;
    this.apartmentView.apartment = this.apartment;
  
    this.apartmentService.updateApartment(this.apartmentView)
      .subscribe(apartmentView => this.apartmentView = apartmentView);
  }
}
