import { Component, OnInit } from '@angular/core';

import { ApartmentView } from 'src/app/core/nswag.generated.service';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';

import { ApartmentUserService, ApartmentSearchService }  from 'src/app/core/nswag.generated.service';
import { UserService } from 'src/app/core/nswag.generated.service';

import { ApartmentDTO } from 'src/app/core/nswag.generated.service';
import { AddressDTO } from 'src/app/core/nswag.generated.service';
import { CountryDTO } from 'src/app/core/nswag.generated.service';

import * as jwt_decode from 'jwt-decode';


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
    private searchService: ApartmentSearchService,
    private authService: UserService
  ) { }

  ngOnInit(): void {
    this.getApartment();
    this.getCountries();
  }
  
  countries: CountryDTO[];

  apartmentView: ApartmentView;
  country: CountryDTO;
  address: AddressDTO;
  apartment: ApartmentDTO;

  getApartment(): void {
    const id = this.route.snapshot.paramMap.get('id');
    this.apartmentService.getApartmentById(id)
      .subscribe(apartmentView => {this.apartmentView = apartmentView;

      this.country = this.apartmentView?.country;
      this.address = this.apartmentView?.address;
      this.apartment = this.apartmentView?.apartment;
      });
  }

  getCountries(): void {
    this.searchService.getAllCountries()
      .subscribe(countries => this.countries = countries);
  }

  get isOwner(){
    var token = this.authService.getToken();
      
    var decodedoken = jwt_decode(token);
    var currentUserId = decodedoken['id'];

    if (currentUserId === this.apartment.ownerId) {
      return true;
    }
    return false;
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

  delete(){
    const id = this.route.snapshot.paramMap.get('id');
    this.apartmentService.deleteApartmentById(id);
  }
}
