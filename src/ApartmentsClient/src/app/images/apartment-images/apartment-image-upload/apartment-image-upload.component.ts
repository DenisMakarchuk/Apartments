import { Component, OnInit } from '@angular/core';
import { ApartmentImageService } from 'src/app/services/nswag.generated.service'
import { ActivatedRoute } from '@angular/router';
import { GetImagesService } from 'src/app/services/get-images.service';

@Component({
  selector: 'app-apartment-image-upload',
  templateUrl: './apartment-image-upload.component.html',
  styleUrls: ['./apartment-image-upload.component.scss']
})
export class ApartmentImageUploadComponent implements OnInit {

  spinning = false;
  isUploaded = false;
  errorMessage: string;

  selectedFile: File;

  constructor(
    private route: ActivatedRoute,
    private imageService: ApartmentImageService,
    private postman: GetImagesService
    ) { }

  ngOnInit(): void {
  }

  onFileChanged(event) {
    this.selectedFile = event.target.files[0];
    this.errorMessage = null;
    this.isUploaded = false;
  }

  onUpload() {
    this.spinning = true;
    this.errorMessage = null;

    if (this.selectedFile) {
      this.imageService.uploadImage(this.selectedFile, this.route.snapshot.paramMap.get('id'))
      .subscribe(addressImage =>{
        this.spinning = false;
        this.isUploaded = true;
        this.postman.go(true);
  
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
    else
    {
      this.spinning = false;
      this.errorMessage = "No image selected!";
    }
  }
}
