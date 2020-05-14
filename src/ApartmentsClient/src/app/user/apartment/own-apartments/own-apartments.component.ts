import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';

import { ApartmentUserService,
  CommentDTO,
  PagedResponseOfApartmentView } from 'src/app/services/nswag.generated.service';

@Component({
  selector: 'app-own-apartments',
  templateUrl: './own-apartments.component.html',
  styleUrls: ['./own-apartments.component.scss']
})
export class OwnApartmentsComponent implements OnInit {

  response: PagedResponseOfApartmentView;
  requestForm: FormGroup;
  pages: number[] = [];

  constructor(
    private apartmentService: ApartmentUserService,
    private fb: FormBuilder
    ) { 
    this.requestForm = this.fb.group({
      pageNumber: 1,
      pageSize: 20
    });
  }

  ngOnInit(): void {
  }

  getOwnApartments(){
    this.apartmentService.getAllApartmentByOwnerId(this.requestForm.value)
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
      this.getOwnApartments();
    }
  }

  nextPage(){
    if (this.response != null && this.response.hasNextPage) {
      this.requestForm.value.pageNumber += 1;
      this.getOwnApartments();
    }
  }

  getPage(page: number){
    if (this.response != null && page > 0 && page <= this.response.totalPages) {
      this.requestForm.value.pageNumber = page;
      this.getOwnApartments();
    }
  }

}
