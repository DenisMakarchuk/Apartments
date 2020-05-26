import { Component, OnInit } from '@angular/core';
import { ApartmentImageService } from 'src/app/services/nswag.generated.service';
import { ActivatedRoute } from '@angular/router';
import { LoggedService } from 'src/app/services/logged.service';
import { ApartmentSearchService }  from 'src/app/services/nswag.generated.service';
import { GetImagesService } from 'src/app/services/get-images.service';
import * as jwt_decode from 'jwt-decode';

@Component({
  selector: 'app-apartment-all-images-names',
  templateUrl: './apartment-all-images-names.component.html',
  styleUrls: ['./apartment-all-images-names.component.scss']
})
export class ApartmentAllImagesNamesComponent implements OnInit {

  spinning = false;
  spinningDel = false;
  errorMessage: string;

  imagesCount: number[] = [];
  currentImageNumber = 1;

  allImages: string[];
  currentImage: string;

  isOwner = false;
  currentApartmentId = this.route.snapshot.paramMap.get('id');

  constructor(
    private route: ActivatedRoute,
    private imageService: ApartmentImageService,
    private searchService: ApartmentSearchService,
    private authService: LoggedService,
    private postman: GetImagesService
  ) { 
    this.postman.go$
    .subscribe(go => {
      if (go) {
        this.getImages();
      }
    });
  }

  ngOnInit(): void {
    this.getImages();
    this.IsOwner();
  }

  IsOwner(){
    this.errorMessage = null;
    this.spinning = true;

    this.searchService.getApartmentById(this.currentApartmentId)
    .subscribe(apartment => {
      this.spinning = false;

      var token = this.authService.getToken();
        
      var decodedoken = jwt_decode(token);
      var currentUserId = decodedoken['id'];
      
      if (apartment.apartment.ownerId === currentUserId) {
        this.isOwner = true;
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
      if (error.status ===  404) {
        this.errorMessage = "Error 404: " + error.response;
      }
      else{
        this.errorMessage = "An error occurred.";
      }
    });
  }

  getImages(){
    this.errorMessage = null;
    this.spinning = true;

    this.imageService.getImageNamesList(this.currentApartmentId)
    .subscribe(allImages =>{
      this.spinning = false;

      this.allImages = allImages;

      this.imagesCount = [];
      for (let index = 1; index <= this.allImages.length; index++) {
        this.imagesCount.push(index);
      };

      this.getImage(this.currentImageNumber);
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
        this.errorMessage = null;
      }
      else{
        this.errorMessage = "An error occurred.";
      }
    })
  }

  previouseImage(){
    if (this.allImages != null && this.currentImageNumber > 1) {
      this.currentImageNumber -= 1;
      this.getImage(this.currentImageNumber);
    }
  }

  nextImage(){
    if (this.allImages != null && this.currentImageNumber < this.allImages.length) {
      this.currentImageNumber += 1;
      this.getImage(this.currentImageNumber);
    }
  }

  getImage(imageNumber: number){
    this.errorMessage = null;
    this.spinning = true;

    if (this.allImages != null && imageNumber > 0 && imageNumber <= this.allImages.length) {
      this.currentImageNumber = imageNumber;
      this.imageService.getImage(this.currentApartmentId, this.allImages[imageNumber-1])
      .subscribe(currentImage => {
        this.spinning = false;
        this.currentImage = currentImage;
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
    this.spinning = false;
  }

  deleteImage(){
    this.errorMessage = null;
    this.spinningDel = true;

    if (this.allImages != null && this.currentImageNumber > 0 && this.currentImageNumber <= this.allImages.length) {

      if (this.currentImageNumber > 1) {
        this.currentImageNumber -= 1;
      }
      
      this.imageService.delete(this.currentApartmentId, this.allImages[this.currentImageNumber-1])
      .subscribe(() => {
        this.spinningDel = false;

        this.currentImage = null;
        this.getImages();
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
      })
    }
  }
}
