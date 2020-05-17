import { Component, OnInit } from '@angular/core';
import { ApartmentImageService } from 'src/app/services/nswag.generated.service';
import { ActivatedRoute } from '@angular/router';
import { LoggedService } from 'src/app/services/logged.service';
import { ApartmentSearchService }  from 'src/app/services/nswag.generated.service';
import * as jwt_decode from 'jwt-decode';

@Component({
  selector: 'app-apartment-all-images-names',
  templateUrl: './apartment-all-images-names.component.html',
  styleUrls: ['./apartment-all-images-names.component.scss']
})
export class ApartmentAllImagesNamesComponent implements OnInit {

  imagesCount: number[] = [];
  currentImageNumber = 1;

  allImages: string[];
  currentImage: string;

  isCustomer = false;
  currentApartmentId = this.route.snapshot.paramMap.get('id');

  constructor(
    private route: ActivatedRoute,
    private imageService: ApartmentImageService,
    private searchService: ApartmentSearchService,
    private authService: LoggedService
  ) { }

  ngOnInit(): void {
    this.getImages();
    this.IsCustomer();
  }

  IsCustomer(){
    this.searchService.getApartmentById(this.currentApartmentId)
    .subscribe(apartment => {
      
      var token = this.authService.getToken();
        
      var decodedoken = jwt_decode(token);
      var currentUserId = decodedoken['id'];
      
      if (apartment.apartment.ownerId === currentUserId) {
        this.isCustomer = true;
      }
    });
  }

  getImages(){
    this.imageService.getImageNamesList(this.currentApartmentId)
    .subscribe(allImages =>{
      this.allImages = allImages;

      this.imagesCount = [];
      for (let index = 1; index <= this.allImages.length; index++) {
        this.imagesCount.push(index);
      };

      this.getImage(this.currentImageNumber);
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
    if (this.allImages != null && imageNumber > 0 && imageNumber <= this.allImages.length) {
      this.currentImageNumber = imageNumber;
      this.imageService.getImage(this.currentApartmentId, this.allImages[imageNumber-1])
      .subscribe(currentImage => this.currentImage = currentImage)
    }
  }

  show(){
    this.getImages();
  }

  deleteImage(){
    if (this.allImages != null && this.currentImageNumber > 0 && this.currentImageNumber <= this.allImages.length) {
      this.imageService.delete(this.currentApartmentId, this.allImages[this.currentImageNumber-1])
      .subscribe(() => {
        this.currentImage = null;
        this.currentImageNumber = 1;
        this.getImages();
      })
    }
  }
}
