import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { OrderUserService, OrderView } from 'src/app/services/nswag.generated.service'
import { ActivatedRoute } from '@angular/router';


@Component({
  selector: 'app-own-order-detail',
  templateUrl: './own-order-detail.component.html',
  styleUrls: ['./own-order-detail.component.scss']
})
export class OwnOrderDetailComponent implements OnInit {

  spinning = false;
  spinningDel = false;
  errorMessage: string;

  order: OrderView;

  constructor(
    private location: Location,
    private route: ActivatedRoute,
    private orderService: OrderUserService
  ) { }

  ngOnInit(): void {
    this.getOrder();
  }

  getOrder(){
    this.errorMessage = null;
    this.spinning = true;

    const id = this.route.snapshot.paramMap.get('id');
    this.orderService.getOrderById(id)
    .subscribe(order => {
      this.spinning = false;

      this.order = order;
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
    })
  }

  delete(){
    this.errorMessage = null;
    this.spinningDel = true;

    const id = this.route.snapshot.paramMap.get('id');
    this.orderService.deleteOrderById(id)
    .subscribe(()=>{
      this.spinningDel = false;

      this.goBack();
    },
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
    this.location.back();
  }

}
