import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { UserAdministrationService,
  IdentityUserAdministrationDTO } from 'src/app/services/nswag.generated.service'

@Component({
  selector: 'app-admin-administration',
  templateUrl: './admin-administration.component.html',
  styleUrls: ['./admin-administration.component.scss']
})
export class AdminAdministrationComponent implements OnInit {

  allAdmins: IdentityUserAdministrationDTO[];
  adminsOnPage: IdentityUserAdministrationDTO[];
  currentAdminPage: number;
  countAdminPages: number;

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
    this.adminService.getAllUsersInRole('Admin')
    .subscribe(users=>{
      this.allAdmins = users;
      this.getPages();
      this.currentAdminPage = 1;

      this.getPage(this.currentAdminPage);
    });
  }

  getPages(){
    var pages = (this.allAdmins?.length/this.itemsCountForm.value.pageSize);
    this.countAdminPages = pages === 0 ? 1 : pages;

    this.pages = [];
    for (let index = 1; index <= this.countAdminPages; index++) {
      this.pages.push(index);
    }
  }

  previousePage(){
    if ((this.currentAdminPage - 1) > 0) {
      this.currentAdminPage -= 1;

      this.getPage(this.currentAdminPage);
    }
  }

  nextPage(){
    if ((this.currentAdminPage + 1) <= this.countAdminPages) {
      this.currentAdminPage += 1;

      this.getPage(this.currentAdminPage);
    }
  }

  getPage(page: number){
    this.currentAdminPage = page;
    var itemFrom = page*this.itemsCountForm.value.pageSize - 20;
    if (itemFrom <= 0) {
      itemFrom = 0;
    }
    this.adminsOnPage = this.allAdmins?.slice(itemFrom, page*this.itemsCountForm.value.pageSize);
  }
}
