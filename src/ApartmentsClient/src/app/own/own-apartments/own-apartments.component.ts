import { Component, OnInit } from '@angular/core';
import { ApartmentUserService,
         AddApartment,
         ApartmentView,
         ApartmentDTO,
         AddressDTO,
         CountryDTO } from 'src/app/core/nswag.generated.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-own-apartments',
  templateUrl: './own-apartments.component.html',
  styleUrls: ['./own-apartments.component.scss']
})
export class OwnApartmentsComponent implements OnInit {

    ownApartments: ApartmentDTO[];

    fakeDto: ApartmentDTO;

  constructor(public router: Router, public ownApartmentService: ApartmentUserService) { }

  ngOnInit(): void {
    this.fakeDto = new ApartmentDTO();
    this.fakeDto.id = "8d5b2779-4ed1-438e-5f32-08d7ed0c2b09";
    this.fakeDto.title = "fake-title";
    this.fakeDto.text = "fake-text";
    this.fakeDto.numberOfRooms = 1;
    this.fakeDto.price = 15;
  }
  
  getOwnApartments(){
    this.ownApartments = [];
    this.ownApartments.push(this.fakeDto);

    //this.ownApartmentService.getAllApartmentByOwnerId()
    //  .subscribe(ownApartments => this.ownApartments = ownApartments);
  }
}
