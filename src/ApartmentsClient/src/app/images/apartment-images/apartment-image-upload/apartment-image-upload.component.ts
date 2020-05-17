import { Component, OnInit } from '@angular/core';
import { ApartmentImageService } from 'src/app/services/nswag.generated.service'
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-apartment-image-upload',
  templateUrl: './apartment-image-upload.component.html',
  styleUrls: ['./apartment-image-upload.component.scss']
})
export class ApartmentImageUploadComponent implements OnInit {

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
    this.imageService.uploadImage(this.selectedFile, this.route.snapshot.paramMap.get('id'))
    .subscribe(addressImage =>{
      this.addressImage = addressImage;
      this.selectedFile = null;
    })
  }
}
