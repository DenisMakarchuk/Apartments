import { Component, OnInit } from '@angular/core';

import { Router } from '@angular/router';

import { FormGroup, FormBuilder } from '@angular/forms';
import { UserService } from 'src/app/services/nswag.generated.service';
import { UserViewModel } from 'src/app/services/nswag.generated.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})

export class LoginComponent implements OnInit {

  spinning = false;
  errorMessage: string;

  loginForm: FormGroup;

  user: UserViewModel;

  constructor( 
    private authService: UserService,
    public fb: FormBuilder,
    public router: Router) { 
      this.loginForm = this.fb.group({
        userName: '',
        password: ''
      });
    }

  ngOnInit(): void {
  }

  login() {
    this.errorMessage = null;
    this.spinning = true;

    if (this.loginForm.valid) {
      this.authService.login(this.loginForm.value)
      .subscribe(user => {
          this.spinning = false;

          this.user = user;

        if (this.user != null) {
          localStorage.setItem('access_token', this.user.token);

          this.loginForm.reset();
          this.router.navigate(['/profile']);
        }
      },
      error=>{
        this.spinning = false;

        if (error.status ===  500) {
          this.errorMessage = "Error 500: Internal Server Error";
        }
        if (error.status ===  400) {
          this.errorMessage = "Error 400: " + error.response;
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
