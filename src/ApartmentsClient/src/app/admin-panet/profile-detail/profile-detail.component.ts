import { Component, OnInit } from '@angular/core';
import { 
  UserAdministrationService,
  UserAdministrationView,
  UserDTOAdministration,
  IdentityUserAdministrationDTO,
  ResultOfUserAdministrationView

} from 'src/app/core/nswag.generated.service';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';

@Component({
  selector: 'app-profile-detail',
  templateUrl: './profile-detail.component.html',
  styleUrls: ['./profile-detail.component.scss']
})
export class ProfileDetailComponent implements OnInit {

  profile: ResultOfUserAdministrationView;
  dataView: UserAdministrationView;
  
  constructor(
    private adminService: UserAdministrationService, 
    private route: ActivatedRoute,
    private location: Location
    ) { }

  ngOnInit(): void {
    this.getProfile();
  }

  getProfile(){
    const id = this.route.snapshot.paramMap.get('id');
    this.adminService.getUserById(id)
    .subscribe(profile => this.profile = profile);
  }

  addToAdmin(){
    const id = this.route.snapshot.paramMap.get('id');
    this.adminService.changeRoleToAdmin(id)
    .subscribe(()=>this.goBack());
  }

  removeFromAdmin(){
    const id = this.route.snapshot.paramMap.get('id');
    this.adminService.changeRoleToUser(id)
    .subscribe(()=>this.goBack());
  }

  delete(){
    const id = this.route.snapshot.paramMap.get('id');
    this.adminService.deleteUserById(id)
    .subscribe(()=>this.goBack());

    
  }

  goBack(): void {
    this.location.back();
  }

}
