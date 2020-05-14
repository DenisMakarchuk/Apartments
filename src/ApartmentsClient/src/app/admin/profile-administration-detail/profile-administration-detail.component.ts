import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { UserAdministrationService,
  UserAdministrationView } from 'src/app/services/nswag.generated.service';

@Component({
  selector: 'app-profile-administration-detail',
  templateUrl: './profile-administration-detail.component.html',
  styleUrls: ['./profile-administration-detail.component.scss']
})
export class ProfileAdministrationDetailComponent implements OnInit {

  user: UserAdministrationView;

  constructor(
    private userService: UserAdministrationService,
    private location: Location,
    private route: ActivatedRoute
  ) { }

  ngOnInit(): void {
    this.getUser();
  }

  goBack(){
    this.location.back();
  }

  getUser(){
    const id = this.route.snapshot.paramMap.get('id');
    this.userService.getUserById(id)
    .subscribe(user => this.user = user)
  }

  changeRole(){
    const id = this.route.snapshot.paramMap.get('id');
    this.userService.changeRole(id, this.user.roles)
    .subscribe(user => this.user = user)
  }

  deleteUser(){
    const id = this.route.snapshot.paramMap.get('id');
    this.userService.deleteUserById(id)
    .subscribe(()=>this.goBack())
  }
}
