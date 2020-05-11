import { Component, OnInit } from '@angular/core';

import { ApartmentView } from 'src/app/services/nswag.generated.service';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';

import { ApartmentUserService, ApartmentSearchService, CommentUserService, OrderUserService }  from 'src/app/services/nswag.generated.service';
import { UserService } from 'src/app/services/nswag.generated.service';

import { ApartmentDTO, CommentDTO } from 'src/app/services/nswag.generated.service';
import { AddressDTO } from 'src/app/services/nswag.generated.service';
import { CountryDTO, AddOrder, OrderView } from 'src/app/services/nswag.generated.service';
import { LoggedService } from 'src/app/services/logged.service';
import { PagedRequestOfSearchParameters, PagedResponseOfApartmentSearchView, PagedRequestOfString } from 'src/app/services/nswag.generated.service';
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
  comments: PagedRequestOfString;

  addOrder: AddOrder;
  orderViwew: OrderView;

  apartmentView: ApartmentView;
  country: CountryDTO;
  address: AddressDTO;
  apartment: ApartmentDTO;

  isUpdating = false;

  apartmentComments: CommentDTO[];

  constructor(
    public router: Router,
    private orderService: OrderUserService,
    private route: ActivatedRoute,
    private apartmentService: ApartmentUserService,
    private commentService: CommentUserService,
    private location: Location,
    private searchService: ApartmentSearchService,
    private authService: LoggedService,
    private postman: SearchParametersService
  ) { 
    this.postman.request$
    .subscribe(request => this.request = request);
  }

  ngOnInit(): void {
    this.getApartment();
    this.postman.getRequestInfo();
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

  get canMakeOrder(){
    if (this.request?.data?.needDates?.length > 0) {
      return true;
    };
    return false;
  }

  //updating(){
  //  this.isUpdating = true;
  //}

  //getComments(){
  //  const id = this.route.snapshot.paramMap.get('id');
 //   this.commentService.getAllCommentsByApartmentId(id)
  //  .subscribe(comments => this.apartmentComments = comments);
  //}

  getApartment(): void {
    const id = this.route.snapshot.paramMap.get('id');
    this.apartmentService.getApartmentById(id)
      .subscribe(apartmentView => {this.apartmentView = apartmentView;

      this.country = this.apartmentView?.country;
      this.address = this.apartmentView?.address;
      this.apartment = this.apartmentView?.apartment;
      });
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
    .subscribe(orderViwew => {
      this.orderViwew = orderViwew;
      this.router.navigate(['/order/:orderViwew.order.id']);
    });
  }

  goBack(): void {
    this.location.back();
  }
}
