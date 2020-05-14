import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FormGroup, FormBuilder } from '@angular/forms';
import { OrderUserService, PagedResponseOfOrderView} from 'src/app/services/nswag.generated.service';

@Component({
  selector: 'app-apartment-orders',
  templateUrl: './apartment-orders.component.html',
  styleUrls: ['./apartment-orders.component.scss']
})
export class ApartmentOrdersComponent implements OnInit {

  response: PagedResponseOfOrderView;
  requestForm: FormGroup;
  pages: number[] = [];

  constructor(
    private route: ActivatedRoute,
    private orderService: OrderUserService,
    private fb: FormBuilder
  ) { 
    this.requestForm = this.fb.group({
      data: this.route.snapshot.paramMap.get('id'),
      pageNumber: 1,
      pageSize: 20
    });
  }
  ngOnInit(): void {
  }

  getOrders(){
    this.orderService.getAllOrdersByApartmentId(this.requestForm.value)
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
      this.getOrders();
    }
  }

  nextPage(){
    if (this.response != null && this.response.hasNextPage) {
      this.requestForm.value.pageNumber += 1;
      this.getOrders();
    }
  }

  getPage(page: number){
    if (this.response != null && page > 0 && page <= this.response.totalPages) {
      this.requestForm.value.pageNumber = page;
      this.getOrders();
    }
  }

}
