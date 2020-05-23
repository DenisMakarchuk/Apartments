import { Component, OnInit } from '@angular/core';
import { ApartmentImageService } from 'src/app/services/nswag.generated.service'
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-apartment-image-upload',
  templateUrl: './apartment-image-upload.component.html',
  styleUrls: ['./apartment-image-upload.component.scss']
})
export class ApartmentImageUploadComponent implements OnInit {

  spinning = false;
  errorMessage: string;

  addressImage: string;

  selectedFile: File;

  constructor(
    private route: ActivatedRoute,
    private imageService: ApartmentImageService
    ) { }

  ngOnInit(): void {
  }

  onFileChanged(event) {
    this.selectedFile = event.target.files[0];
    this.addressImage = null;
  }

  onUpload() {
    this.spinning = true;
    this.errorMessage = null;

    this.imageService.uploadImage(this.selectedFile, this.route.snapshot.paramMap.get('id'))
    .subscribe(addressImage =>{
      this.spinning = false;

      this.addressImage = addressImage;
      this.selectedFile = null;
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
    })
  }
}
