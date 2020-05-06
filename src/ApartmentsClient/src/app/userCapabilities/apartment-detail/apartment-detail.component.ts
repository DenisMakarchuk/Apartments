import { Component, OnInit } from '@angular/core';

import { ApartmentView } from 'src/app/core/nswag.generated.service';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';

import { ApartmentUserService, ApartmentSearchService, CommentUserService, OrderUserService }  from 'src/app/core/nswag.generated.service';
import { UserService } from 'src/app/core/nswag.generated.service';

import { ApartmentDTO, CommentDTO } from 'src/app/core/nswag.generated.service';
import { AddressDTO } from 'src/app/core/nswag.generated.service';
import { CountryDTO } from 'src/app/core/nswag.generated.service';

import { Router } from '@angular/router';

import * as jwt_decode from 'jwt-decode';


@Component({
  selector: 'app-apartment-detail',
  templateUrl: './apartment-detail.component.html',
  styleUrls: ['./apartment-detail.component.scss']
})
export class ApartmentDetailComponent implements OnInit {

  constructor(
    public router: Router,
    private orderService: OrderUserService,
    private route: ActivatedRoute,
    private apartmentService: ApartmentUserService,
    private commentService: CommentUserService,
    private location: Location,
    private searchService: ApartmentSearchService,
    private authService: UserService
  ) { 
    //this.searchService.getDates().subscribe(dates => this.dates = dates);
  }

  ngOnInit(): void {
    this.getApartment();
    this.getCountries();
    this.getComments();
    this.getDates();
  }
  
  countries: CountryDTO[];

  apartmentView: ApartmentView;
  country: CountryDTO;
  address: AddressDTO;
  apartment: ApartmentDTO;

  isUpdating = false;

  apartmentComments: CommentDTO[];
  dates: Date[];

  get isOwner(){
    var token = this.authService.getToken();
      
    var decodedoken = jwt_decode(token);
    var currentUserId = decodedoken['id'];

    if (currentUserId === this.apartment.ownerId) {
      return true;
    }
    return false;
  }

  getDates(){
    this.searchService.onClick
    .subscribe(dates=>{
      this.dates = dates;
      console.log(dates);
    })
  }

  updating(){
    this.isUpdating = true;
  }

  getCountries(): void {
    this.searchService.getAllCountries()
      .subscribe(countries => this.countries = countries);
  }

  getComments(){
    const id = this.route.snapshot.paramMap.get('id');
    this.commentService.getAllCommentsByApartmentId(id)
    .subscribe(comments => this.apartmentComments = comments);
  }

  getApartment(): void {
    const id = this.route.snapshot.paramMap.get('id');
    this.apartmentService.getApartmentById(id)
      .subscribe(apartmentView => {this.apartmentView = apartmentView;

      this.country = this.apartmentView?.country;
      this.address = this.apartmentView?.address;
      this.apartment = this.apartmentView?.apartment;
      });
  }

  save(): void {
    this.apartmentView.country = this.country;
    this.apartmentView.address = this.address;
    this.apartmentView.address.countryId = this.country.id;
    this.apartmentView.apartment = this.apartment;
  
    this.apartmentService.updateApartment(this.apartmentView)
      .subscribe(apartmentView => {
        this.apartmentView = apartmentView;
        
        this.isUpdating = false;
      });
  }

  delete(){
    const id = this.route.snapshot.paramMap.get('id');
    this.apartmentService.deleteApartmentById(id)
      .subscribe(()=>{
        this.apartmentView = null;
        this.country = null;
        this.address = null;
        this.apartment = null;
    
        this.goBack();
      });
  }

  goBack(): void {
    this.location.back();
  }
}
