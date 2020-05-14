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
    const id = this.route.snapshot.paramMap.get('id');
    this.orderService.getOrderById(id)
    .subscribe(order => this.order = order)
  }

  delete(){
    const id = this.route.snapshot.paramMap.get('id');
    this.orderService.deleteOrderById(id)
    .subscribe(()=>this.goBack());
  }

  goBack(){
    this.location.back();
  }

}
