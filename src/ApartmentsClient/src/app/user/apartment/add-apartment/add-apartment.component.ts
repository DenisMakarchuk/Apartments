import { Component, OnInit } from '@angular/core';
import { NgForm, FormControl, FormGroup, FormBuilder } from '@angular/forms';

import { ApartmentView } from 'src/app/services/nswag.generated.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Location } from '@angular/common';

import { ApartmentUserService }  from 'src/app/services/nswag.generated.service';
import { ApartmentSearchService } from 'src/app/services/nswag.generated.service';

import { ApartmentDTO, AddApartment } from 'src/app/services/nswag.generated.service';
import { AddressDTO, AddAddress } from 'src/app/services/nswag.generated.service';
import { CountryDTO } from 'src/app/services/nswag.generated.service';

@Component({
  selector: 'app-add-apartment',
  templateUrl: './add-apartment.component.html',
  styleUrls: ['./add-apartment.component.scss']
})
export class AddApartmentComponent implements OnInit {

  countries: CountryDTO[];

  apartmentView: ApartmentView;
  country: CountryDTO;

  address: AddressDTO;
  addAddress: AddAddress;

  apartment: ApartmentDTO;
  addApartment: AddApartment;

  apartmentForm: FormGroup;
  addressForm: FormGroup;

  countryId: string;
  city: string;
  street: string;
  home: string;
  numberOfApartment: number;

  title: string;
  text: string;
  area: number;
  isOpen: boolean;
  price: number;
  numberOfRooms: number;

  constructor(
    private route: ActivatedRoute,
    private apartmentService: ApartmentUserService,
    private formBuilder: FormBuilder,
    private avRoute: ActivatedRoute, 
    private router: Router,
    private location: Location,
    private searchService: ApartmentSearchService
  ) { 
    this.addressForm = this.formBuilder.group({
      countryId: '',
      city: '',
      street: '',
      home: '',
      numberOfApartment: 0
    })
    
    this.apartmentForm = this.formBuilder.group({
      address: this.addressForm,
      title: '',
      text: '',
      area: 0,
      isOpen: true,
      price: 0,
      numberOfRooms: 0
      })
  }

  ngOnInit(): void {
    this.getCountries();
  }

  getCountries(): void {
    this.searchService.getAllCountries()
      .subscribe(countries => this.countries = countries);
  }

  add(){
    this.apartmentService.createApartment(this.apartmentForm.value)
      .subscribe(apartmentView => 
        {
          this.apartmentView = apartmentView;
          this.router.navigate(['/apartment', apartmentView.apartment.id ]);
        });
  }

  goBack(): void {
    this.location.back();
  }
}
