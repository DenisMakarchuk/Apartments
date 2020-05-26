import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { ApartmentImageService } from 'src/app/services/nswag.generated.service';

import { ApartmentUserService,
  PagedResponseOfApartmentView } from 'src/app/services/nswag.generated.service';

@Component({
  selector: 'app-own-apartments',
  templateUrl: './own-apartments.component.html',
  styleUrls: ['./own-apartments.component.scss']
})
export class OwnApartmentsComponent implements OnInit {

  spinning = false;
  errorMessage: string;

  response: PagedResponseOfApartmentView;
  requestForm: FormGroup;
  pages: number[] = [];

  mainImages: {[id:string]: string} = { };

  constructor(
    private apartmentService: ApartmentUserService,
    private fb: FormBuilder,
    private imageService: ApartmentImageService
    ) { 
    this.requestForm = this.fb.group({
      pageNumber: 1,
      pageSize: 20
    });
  }

  ngOnInit(): void {
  }

  getOwnApartments(){
    this.errorMessage = null;
    this.spinning = true;

    this.apartmentService.getAllApartmentByOwnerId(this.requestForm.value)
    .subscribe(response => {

      this.spinning = false;
      this.response = response;

      if (this.response.data) {
        for(let ap of this.response.data){
          this.getImages(ap.apartment.id)
        }
      }
    
      this.pages = [];
      for (let index = 1; index <= response.totalPages; index++) {
        this.pages.push(index);
      }
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
      else{
        this.errorMessage = "An error occurred.";
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

  getImages(id: string){
    this.imageService.getImageNamesList(id + 'Mini')
    .subscribe(allImages =>{
      if (allImages != null && allImages.length > 0) {
        this.imageService.getImage(id + 'Mini', allImages[0])
        .subscribe(currentImage => {
        this.mainImages[id] = currentImage;
        })
      }
    })
  }
}
