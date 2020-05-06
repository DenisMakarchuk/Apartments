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

  constructor(public router: Router, public ownApartmentService: ApartmentUserService) { }

  ngOnInit(): void {
  }
  
  getOwnApartments(){
    this.ownApartmentService.getAllApartmentByOwnerId()
      .subscribe(ownApartments => this.ownApartments = ownApartments);
  }
}
