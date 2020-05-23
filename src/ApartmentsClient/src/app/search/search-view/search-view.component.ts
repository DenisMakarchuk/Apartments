import { Component, OnInit } from '@angular/core';
import { ApartmentImageService } from 'src/app/services/nswag.generated.service';

import { ApartmentSearchService } from 'src/app/services/nswag.generated.service';
import { LoggedService } from 'src/app/services/logged.service';

import { PagedRequestOfSearchParameters, PagedResponseOfApartmentSearchView } from 'src/app/services/nswag.generated.service';
import { SearchParametersService } from 'src/app/services/search-parameters.service';

@Component({
  selector: 'app-search-view',
  templateUrl: './search-view.component.html',
  styleUrls: ['./search-view.component.scss']
})
export class SearchViewComponent implements OnInit {

  spinning = false;
  errorMessage: string;

  pages: number[] = [];

  request: PagedRequestOfSearchParameters;
  response: PagedResponseOfApartmentSearchView;

  mainImages: {[id:string]: string} = { };

  constructor(
    private searchService: ApartmentSearchService,
    private authService: LoggedService,
    private postman: SearchParametersService,
    private imageService: ApartmentImageService
  ) {
    this.postman.request$
    .subscribe(request => this.request = request);

    this.postman.response$
    .subscribe(response => {
      this.response = response;

      if (this.response?.data) {
        for(let ap of this.response.data){
          this.getImages(ap.apartment.id)
        }

        this.pages = [];
        for (let index = 1; index <= response.totalPages; index++) {
          this.pages.push(index);
        }
      }
      
    });
  }

  ngOnInit(): void {
    this.postman.getRequestInfo();
    this.postman.getResponseInfo();
  }

  newPage(): void {
    this.errorMessage = null;
    this.spinning = true;

    this.searchService.getAllApartments(this.request)
      .subscribe(response => {

        this.spinning = false;
        this.response = response;

        this.pages = [];
        for (let index = 1; index <= response.totalPages; index++) {
          this.pages.push(index);

        this.postman.setSearchInfo(this.request, this.response);
        this.postman.getRequestInfo();
        this.postman.getResponseInfo();
        };
      },
      error=>{
        this.spinning = false;

        if (error.status ===  500) {
          this.errorMessage = "Error 500: Internal Server Error";
        }
        if (error.status ===  400) {
          this.errorMessage = "Error 400: " + error.title;
        }
        else{
          this.errorMessage = "An error occurred.";
        }
      });
  }

  get isLoggedIn(): boolean {
    return this.authService.isLoggedIn;
  }

  previousePage(){
    if (this.response != null && this.response.hasPreviousPage) {
      this.request.pageNumber -= 1;
      this.newPage();
    }
  }

  nextPage(){
    if (this.response != null && this.response.hasNextPage) {
      this.request.pageNumber += 1;
      this.newPage();
    }
  }

  getPage(page: number){
    if (this.response != null && page > 0 && page <= this.response.totalPages) {
      this.request.pageNumber = page;
      this.newPage();
    }
  }

  getImages(id: string){
    this.imageService.getImageNamesList(id)
    .subscribe(allImages =>{
      if (allImages != null && allImages.length > 0) {
        this.imageService.getImage(id, allImages[0])
        .subscribe(currentImage => {
        this.mainImages[id] = currentImage;
        })
      }
    })
  }
}
