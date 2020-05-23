import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';

import { ApartmentView } from 'src/app/services/nswag.generated.service';
import { Router } from '@angular/router';
import { Location } from '@angular/common';

import { ApartmentUserService }  from 'src/app/services/nswag.generated.service';
import { ApartmentSearchService } from 'src/app/services/nswag.generated.service';

import { CountryDTO } from 'src/app/services/nswag.generated.service';

@Component({
  selector: 'app-add-apartment',
  templateUrl: './add-apartment.component.html',
  styleUrls: ['./add-apartment.component.scss']
})
export class AddApartmentComponent implements OnInit {

  spinning = false;
  errorMessage: string;

  countries: CountryDTO[];

  apartmentView: ApartmentView;

  apartmentForm: FormGroup;
  addressForm: FormGroup;

  constructor(
    private apartmentService: ApartmentUserService,
    private formBuilder: FormBuilder,
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
    this.errorMessage = null;
    this.spinning = true;

    this.searchService.getAllCountries()
      .subscribe(countries => {
        this.spinning = false;

        this.countries = countries;
      },
      error=>{
        this.spinning = false;

        if (error.status ===  500) {
          this.errorMessage = "Error 500: Internal Server Error";
        }
        if (error.status ===  400) {
          this.errorMessage = "Error 400: " + error.response;
        }
        else{
          this.errorMessage = "Unsuspected Error";
        }
      });
  }

  add(){
    this.errorMessage = null;
    this.spinning = true;

    if (this.apartmentForm.valid) {
      this.apartmentService.createApartment(this.apartmentForm.value)
      .subscribe(apartmentView => 
        {
          this.spinning = false;

          this.apartmentView = apartmentView;
          this.router.navigate(['/apartment', apartmentView.apartment.id ]);
        },
        error=>{
          this.spinning = false;
  
          if (error.status ===  500) {
            this.errorMessage = "Error 500: Internal Server Error";
          }
          if (error.status ===  400) {
            this.errorMessage = "Error 400: " + error.response;
          }
          if (error.status ===  403) {
            this.errorMessage = "Error 403: You are not authorized";
          }
          else{
            this.errorMessage = "Unsuspected Error";
          }
        });
    }
    else
    {
      this.spinning = false;
      this.errorMessage = "Invalid data entry";
    }
  }

  goBack(): void {
    this.location.back();
  }
}
