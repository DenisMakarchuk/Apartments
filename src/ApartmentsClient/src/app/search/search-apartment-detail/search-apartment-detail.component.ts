import { Component, OnInit } from '@angular/core';

import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';

import { ApartmentSearchService, OrderUserService }  from 'src/app/services/nswag.generated.service';
import { ApartmentSearchView } from 'src/app/services/nswag.generated.service';

import { AddOrder, OrderDTO } from 'src/app/services/nswag.generated.service';
import { PagedRequestOfSearchParameters } from 'src/app/services/nswag.generated.service';
import { SearchParametersService } from 'src/app/services/search-parameters.service';

import { Router } from '@angular/router';

import * as jwt_decode from 'jwt-decode';

@Component({
  selector: 'app-search-apartment-detail',
  templateUrl: './search-apartment-detail.component.html',
  styleUrls: ['./search-apartment-detail.component.scss']
})
export class SearchApartmentDetailComponent implements OnInit {

  spinning = false;
  spinningOrder = false;
  errorMessage: string;

  request: PagedRequestOfSearchParameters;

  addOrder: AddOrder;
  orderDto: OrderDTO;

  apartment: ApartmentSearchView;

  constructor(
    public router: Router,
    private orderService: OrderUserService,
    private route: ActivatedRoute,
    private location: Location,
    private searchService: ApartmentSearchService,
    private postman: SearchParametersService
  ) { 
    this.postman.request$
    .subscribe(request => this.request = request);
  }

  ngOnInit(): void {
    this.getApartment();
    this.postman.getRequestInfo();
  }

  get canMakeOrder(){
    if (this.request?.data?.needDates?.length > 0) {
      return true;
    };
    return false;
  }

  getApartment(): void {
    this.errorMessage = null;
    this.spinning = true;

    this.searchService.getApartmentById(this.getApartmentId())
      .subscribe(apartment => {

        this.spinning = false;
        this.apartment = apartment
      },
      error=>{
        this.spinning = false;
  
        if (error.status ===  500) {
          this.errorMessage = "Error 500: Internal Server Error";
        }
        if (error.status ===  400) {
          this.errorMessage = "Error 400: " + error.response;
        }
        if (error.status ===  404) {
          this.errorMessage = "Error 404: " + error.response;
        }
        else{
          this.errorMessage = "An error occurred.";
        }
      });
  }

  getApartmentId(): string {
    const id = this.route.snapshot.paramMap.get('id');
    return id;
  }

  formAnOrder(){
    this.errorMessage = null;
    this.spinningOrder = true;

    this.addOrder = new AddOrder();
    this.addOrder.apartmentId = this.getApartmentId();
    this.addOrder.dates = this.request.data.needDates;

    this.orderService.formationOrder(this.addOrder)
    .subscribe(orderDto => {

      this.spinningOrder = false;
      this.orderDto = orderDto;
    },
    error=>{
      this.spinningOrder = false;
  
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

  makeOrder(){
    this.errorMessage = null;
    this.spinningOrder = true;

    this.addOrder = new AddOrder();
    this.addOrder.apartmentId = this.getApartmentId();
    this.addOrder.dates = this.request.data.needDates;

    this.orderService.createOrder(this.addOrder)
    .subscribe(orderView => {

      this.spinningOrder = false;
      this.router.navigate(['/order', orderView.order.id ]);
    },
    error=>{
      this.spinningOrder = false;
  
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

  goBack(): void {
    this.location.back();
  }
}
