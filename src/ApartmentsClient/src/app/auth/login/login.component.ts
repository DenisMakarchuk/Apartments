import { Component, OnInit } from '@angular/core';
import { UserLoginRequest } from 'src/app/core/nswag.generated.service';
import { UserDTO } from 'src/app/core/nswag.generated.service';

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

  email: string;
  password: string;

  constructor( 
    private authService: UserService,
    public fb: FormBuilder,
    public router: Router) { 
      this.loginForm = this.fb.group({
        email: '',
        password: ''
      });
    }

  ngOnInit(): void {
  }

  login() {
    if (this.loginForm.valid) {
      this.authService.login(this.loginForm.value)
      .subscribe(user => this.user = user);
  
      if (this.user != null) {
        localStorage.setItem('access_token', this.user.token)
        this.currentUser = this.user;
  
        this.loginForm.reset();
        this.router.navigate(['/profile']);
      }
    }
  }


}
