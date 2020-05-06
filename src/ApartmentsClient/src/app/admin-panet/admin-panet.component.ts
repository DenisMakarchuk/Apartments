import { Component, OnInit } from '@angular/core';
import { 
  UserAdministrationService,

  IdentityUserAdministrationDTO

} from 'src/app/core/nswag.generated.service';



@Component({
  selector: 'app-admin-panet',
  templateUrl: './admin-panet.component.html',
  styleUrls: ['./admin-panet.component.scss']
})
export class AdminPanetComponent implements OnInit {

  users: IdentityUserAdministrationDTO[];
  admins: IdentityUserAdministrationDTO[];

  constructor(private adminService: UserAdministrationService) { }

  ngOnInit(): void {
  }

  getUsers(){
    this.adminService.getAllUsersInRole('User')
    .subscribe(users=>this.users = users);
  }

  getAdmins(){
    this.adminService.getAllUsersInRole('Admin')
    .subscribe(admins=>this.admins = admins);
  }

}
