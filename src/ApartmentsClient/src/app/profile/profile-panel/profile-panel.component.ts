import { Component, OnInit } from '@angular/core';
import { UserService } from 'src/app/services/nswag.generated.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-profile-panel',
  templateUrl: './profile-panel.component.html',
  styleUrls: ['./profile-panel.component.scss']
})
export class ProfilePanelComponent implements OnInit {

  clicker1 = false;
  clicker2 = false;
  clicker3 = false;

  isDeletion = false;

  spinning = false;
  errorMessage: string;

  deletionForm: FormGroup;

  constructor(
    private service: UserService,
    public fb: FormBuilder,
    public router: Router) { 
      this.deletionForm = this.fb.group({
        userName: ['', [Validators.required]],
        password: ['', [Validators.required, Validators.minLength(8)]]
    });
  }

  ngOnInit(): void {
  }

  get _userName(){
    return this.deletionForm.get('userName');
  }

  get _password(){
    return this.deletionForm.get('password');
  }

  changeClicker1(){
    if (this.clicker1) {
      this.clicker1 = false;
    }
    else
    {
      this.clicker1 = true;
    }
  }

  changeClicker2(){
    if (this.clicker2) {
      this.clicker2 = false;
    }
    else
    {
      this.clicker2 = true;
    }
  }

  changeClicker3(){
    if (this.clicker3) {
      this.clicker3 = false;
    }
    else
    {
      this.clicker3 = true;
    }
  }

  goDelete(){
    this.errorMessage = null;
    this.isDeletion = true;
  }

  goBack(){
    this.errorMessage = null;
    this.isDeletion = false;
  }

  deleteAccount(){
    this.errorMessage = null;
    this.spinning = true;

    if (this.deletionForm.valid) {
      this.service.delete(this.deletionForm.value)
      .subscribe(() => {
          this.spinning = false;

          this.router.navigate(['/search']);
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
        if (error.status ===  404) {
          this.errorMessage = "Error 404: " + error.response;
        }
        else{
          this.errorMessage = "Unsuspected Error";
        }
      });
    }
    else
    {
      this.spinning = false;
      this.errorMessage = "Invalid data entry";
    }
  }
}
