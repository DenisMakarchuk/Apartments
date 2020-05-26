import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { ApartmentUserService, ApartmentView, ApartmentSearchService, CountrySearchDTO }  from 'src/app/services/nswag.generated.service';

@Component({
  selector: 'app-own-apartment-detail',
  templateUrl: './own-apartment-detail.component.html',
  styleUrls: ['./own-apartment-detail.component.scss']
})
export class OwnApartmentDetailComponent implements OnInit {

  spinning = false;
  spinningDel = false;
  errorMessage: string;

  apartment: ApartmentView;
  countries: CountrySearchDTO[];
  isUpdating = false;

  constructor(
    private apartmentService: ApartmentUserService,
    private searchService: ApartmentSearchService,
    private location: Location,
    private route: ActivatedRoute
  ) { }

  ngOnInit(): void {
    this.getApartment();
  }

  getCountries(){
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
        this.errorMessage = "An error occurred.";
      }
    });
  }

  getApartment(){
    this.errorMessage = null;
    this.spinning = true;

    const id = this.route.snapshot.paramMap.get('id');
    this.apartmentService.getApartmentById(id)
    .subscribe(apartment => {

      this.spinning = false;
      this.apartment = apartment;
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
        if (error.status ===  404) {
          this.errorMessage = "Error 404: " + error.response;
        }
        else{
          this.errorMessage = "An error occurred.";
        }
      });
  }

  updating(){
    this.isUpdating = true;
    this.getCountries();
  }

  save(): void {
    this.errorMessage = null;
    this.spinning = true;

    this.apartmentService.updateApartment(this.apartment)
      .subscribe(apartment => {
        this.spinning = false;

        this.apartment = apartment;
        this.isUpdating = false;
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
        this.errorMessage = "An error occurred.";
      }
    });
  }

  delete(){
    this.errorMessage = null;
    this.spinningDel = true;

    const id = this.route.snapshot.paramMap.get('id');
    this.apartmentService.deleteApartmentById(id)
    .subscribe(()=>{
      
      this.spinningDel = false;
      this.goBack();},
    error=>{
      this.spinningDel = false;

      if (error.status ===  500) {
        this.errorMessage = "Error 500: Internal Server Error";
      }
      if (error.status ===  400) {
        this.errorMessage = "Error 400: " + error.response;
      }
      if (error.status ===  403) {
        this.errorMessage = "Error 403: You are not authorized";
      }
      if (error.status ===  404) {
        this.errorMessage = "Error 404: " + error.response;
      }
      else{
        this.errorMessage = "An error occurred.";
      }
    });
  }

  goBack(){
    if (this.isUpdating) {
      this.isUpdating = false;
      this.getApartment();
    }
    else{
      this.location.back();
    }
  }

}
