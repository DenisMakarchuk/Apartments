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
    if (this.loginForm.valid) {
      this.authService.login(this.loginForm.value)
      .subscribe(user => 
        {this.user = user;

        if (this.user != null) {
          localStorage.setItem('access_token', this.user.token);

          this.loginForm.reset();
          this.router.navigate(['/profile']);
        }
      });
    }
  }
}
