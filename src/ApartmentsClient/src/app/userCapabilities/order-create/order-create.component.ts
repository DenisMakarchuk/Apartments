import { Component, OnInit } from '@angular/core';
import { NgForm, FormControl, FormGroup, FormBuilder } from '@angular/forms';

import { ApartmentView } from 'src/app/core/nswag.generated.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Location } from '@angular/common';

import { OrderUserService, ApartmentSearchService }  from 'src/app/core/nswag.generated.service';

import { ApartmentDTO, CountryDTO  } from 'src/app/core/nswag.generated.service';
import { AddAddress } from 'src/app/core/nswag.generated.service';
import { AddOrder, OrderView, OrderDTO } from 'src/app/core/nswag.generated.service';


@Component({
  selector: 'app-order-create',
  templateUrl: './order-create.component.html',
  styleUrls: ['./order-create.component.scss']
})
export class OrderCreateComponent implements OnInit {

  dates: Date[];
  newOrder: AddOrder = new AddOrder();

  constructor(    
    public router: Router,
    private orderService: OrderUserService,
    private route: ActivatedRoute,
    private location: Location,
    private searchService: ApartmentSearchService,
  ) { 
    //this.searchService.getDates().subscribe(dates => this.dates = dates);
  }


  ngOnInit(): void {
  }

  addOrder(){
    const id = this.route.snapshot.paramMap.get('id');

    this.newOrder.apartmentId = id;
    this.newOrder.dates = this.dates;

    this.orderService.createOrder(this.newOrder);
  }

}
