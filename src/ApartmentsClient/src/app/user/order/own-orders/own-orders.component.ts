import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { OrderUserService, PagedResponseOfOrderView} from 'src/app/services/nswag.generated.service';

@Component({
  selector: 'app-own-orders',
  templateUrl: './own-orders.component.html',
  styleUrls: ['./own-orders.component.scss']
})
export class OwnOrdersComponent implements OnInit {

  response: PagedResponseOfOrderView;
  requestForm: FormGroup;
  pages: number[] = [];

  constructor(
    private orderService: OrderUserService,
    private fb: FormBuilder
  ) { 
    this.requestForm = this.fb.group({
      pageNumber: 1,
      pageSize: 20
    });
  }

  ngOnInit(): void {
  }

  getOwnOrders(){
    this.orderService.getAllOrdersByCustomerId(this.requestForm.value)
    .subscribe(response => {
      this.response = response;
    
      this.pages = [];
      for (let index = 1; index <= response.totalPages; index++) {
        this.pages.push(index);
      }
    });
  }

  previousePage(){
    if (this.response != null && this.response.hasPreviousPage) {
      this.requestForm.value.pageNumber -= 1;
      this.getOwnOrders();
    }
  }

  nextPage(){
    if (this.response != null && this.response.hasNextPage) {
      this.requestForm.value.pageNumber += 1;
      this.getOwnOrders();
    }
  }

  getPage(page: number){
    if (this.response != null && page > 0 && page <= this.response.totalPages) {
      this.requestForm.value.pageNumber = page;
      this.getOwnOrders();
    }
  }
}
