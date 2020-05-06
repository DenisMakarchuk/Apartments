import { Component, OnInit } from '@angular/core';
import { OrderUserService,
  OrderView,
  OrderDTO,
  ApartmentDTO,
  AddressDTO,
  CountryDTO } from 'src/app/core/nswag.generated.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-own-orders',
  templateUrl: './own-orders.component.html',
  styleUrls: ['./own-orders.component.scss']
})
export class OwnOrdersComponent implements OnInit {

  ownOrders: OrderView[];

  order: OrderDTO;
  apartment: ApartmentDTO;
  address: AddressDTO;
  country: CountryDTO;

  constructor(public router: Router, public orderService: OrderUserService) { }

  ngOnInit(): void {
    this.getOwnOrders();
  }

  getOwnOrders(){
    this.orderService.getAllOrdersByCustomerId()
      .subscribe(ownOrders => this.ownOrders = ownOrders);
  }
}
