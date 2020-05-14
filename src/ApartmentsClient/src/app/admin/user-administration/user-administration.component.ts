import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { UserAdministrationService,
  IdentityUserAdministrationDTO } from 'src/app/services/nswag.generated.service'

@Component({
  selector: 'app-user-administration',
  templateUrl: './user-administration.component.html',
  styleUrls: ['./user-administration.component.scss']
})
export class UserAdministrationComponent implements OnInit {

  allUsers: IdentityUserAdministrationDTO[];
  usersOnPage: IdentityUserAdministrationDTO[];
  currentUserPage: number;
  countUserPages: number;

  itemsCountForm: FormGroup;
  pages: number[] = [];

  constructor(
    private adminService: UserAdministrationService,
    private fb: FormBuilder
    ) { 
      this.itemsCountForm = this.fb.group({
        pageSize: 20
      });
    }

  ngOnInit(): void {
  }

  getUsers(){
    this.adminService.getAllUsersInRole('User')
    .subscribe(users=>{
      this.allUsers = users;
      this.getPages();
      this.currentUserPage = 1;

      this.getPage(this.currentUserPage);
    });
  }

  getPages(){
    var pages = (this.allUsers?.length/this.itemsCountForm.value.pageSize);
    this.countUserPages = pages === 0 ? 1 : pages;

    this.pages = [];
    for (let index = 1; index <= this.countUserPages; index++) {
      this.pages.push(index);
    }
  }

  previousePage(){
    if ((this.currentUserPage - 1) > 0) {
      this.currentUserPage -= 1;

      this.getPage(this.currentUserPage);
    }
  }

  nextPage(){
    if ((this.currentUserPage + 1) <= this.countUserPages) {
      this.currentUserPage += 1;

      this.getPage(this.currentUserPage);
    }
  }

  getPage(page: number){
    this.currentUserPage = page;
    var itemFrom = page*this.itemsCountForm.value.pageSize - 20;
    if (itemFrom <= 0) {
      itemFrom = 0;
    }
    this.usersOnPage = this.allUsers?.slice(itemFrom, page*this.itemsCountForm.value.pageSize);
  }

}
