import { Component, OnInit } from '@angular/core';

import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';

import { ApartmentSearchService, OrderUserService }  from 'src/app/services/nswag.generated.service';
import { ApartmentSearchView } from 'src/app/services/nswag.generated.service';

import { AddOrder, OrderView } from 'src/app/services/nswag.generated.service';
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

  request: PagedRequestOfSearchParameters;

  addOrder: AddOrder;
  orderView: OrderView;

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
    this.searchService.getApartmentById(this.getApartmentId())
      .subscribe(apartment => this.apartment = apartment);
  }

  getApartmentId(): string {
    const id = this.route.snapshot.paramMap.get('id');
    return id;
  }

  makeOrder(){
    this.addOrder = new AddOrder();
    this.addOrder.apartmentId = this.getApartmentId();
    this.addOrder.dates = this.request.data.needDates;

    this.orderService.createOrder(this.addOrder)
    .subscribe(orderView => {
      this.orderView = orderView;
      this.router.navigate(['/order', orderView.order.id ]);
    });
  }

  goBack(): void {
    this.location.back();
  }
}
