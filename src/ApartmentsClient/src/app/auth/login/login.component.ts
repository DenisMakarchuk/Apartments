import { Component, OnInit } from '@angular/core';
import { UserLoginRequest } from 'src/app/core/nswag.generated.service';

import { Router } from '@angular/router';

import { FormGroup, FormBuilder } from '@angular/forms';
import { UserService } from 'src/app/core/nswag.generated.service';
import { UserViewModel } from 'src/app/core/nswag.generated.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  loginForm: FormGroup;
  loginRequest: UserLoginRequest;
  user: UserViewModel;
  currentUser = {};

  constructor( 
    private authService: UserService,
    fb: FormBuilder,
    public router: Router) { 
      this.loginForm = fb.group({
        name: [''],
        password: ['']
      });
    }

  ngOnInit(): void {
  }

  login() {
    if (this.loginForm.valid) {
      const data = this.loginForm.value;

      this.loginRequest = new UserLoginRequest();

      this.loginRequest.email = data.name;
      this.loginRequest.password = data.password;

      this.authService.login(this.loginRequest)
        .subscribe(user => this.user = user);
          localStorage.setItem('access_token', this.user.token)
          this.currentUser = this.user;

          this.router.navigate(['profile']);
          this.loginForm.reset();
  
    }
  }

  getToken() {
    return localStorage.getItem('access_token');
  }

  get isLoggedIn(): boolean {
    let authToken = localStorage.getItem('access_token');
    return (authToken !== null) ? true : false;
  }

  doLogout() {
    let removeToken = localStorage.removeItem('access_token');
    if (removeToken == null) {
      this.router.navigate(['login']);
    }
  }
}
