import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { ApartmentUserService, ApartmentView, ApartmentSearchService, CountrySearchDTO }  from 'src/app/services/nswag.generated.service';

@Component({
  selector: 'app-own-apartment-detail',
  templateUrl: './own-apartment-detail.component.html',
  styleUrls: ['./own-apartment-detail.component.scss']
})
export class OwnApartmentDetailComponent implements OnInit {

  apartment: ApartmentView;
  countries: CountrySearchDTO[];
  isUpdating = false;

  constructor(
    private apartmentService: ApartmentUserService,
    private searchService: ApartmentSearchService,
    private location: Location,
    private route: ActivatedRoute
  ) { }

  ngOnInit(): void {
    this.getApartment();
  }

  getCountries(){
    this.searchService.getAllCountries()
    .subscribe(countries => this.countries = countries);
  }

  getApartment(){
    const id = this.route.snapshot.paramMap.get('id');
    this.apartmentService.getApartmentById(id)
    .subscribe(apartment => this.apartment = apartment);
  }

  updating(){
    this.isUpdating = true;
    this.getCountries();
  }

  save(): void {
    this.apartmentService.updateApartment(this.apartment)
      .subscribe(apartment => {
        this.apartment = apartment;
        this.isUpdating = false;
    });
  }

  delete(){
    const id = this.route.snapshot.paramMap.get('id');
    this.apartmentService.deleteApartmentById(id)
    .subscribe(()=>this.goBack());
  }

  goBack(){
    if (this.isUpdating) {
      this.isUpdating = false;
    }
    else{
      this.location.back();
    }
  }

}
