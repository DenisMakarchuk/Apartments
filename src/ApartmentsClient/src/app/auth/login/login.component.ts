import { Component, OnInit } from '@angular/core';
import { UserService } from 'src/app/core/nswag.generated.service';
import { UserLoginRequest } from 'src/app/core/nswag.generated.service';

import { FormGroup, FormBuilder } from '@angular/forms';


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  loginForm: FormGroup;
  loginRequest: UserLoginRequest;

  constructor( 
    private authService: UserService,
    fb: FormBuilder) { 
      this.loginForm = fb.group({
        name: [''],
        password: ['']
      });
    }

  ngOnInit(): void {
  }

  onSubmit() {
    if (this.loginForm.valid) {
      const data = this.loginForm.value;

      this.loginRequest.email = data.name;
      this.loginRequest.password = data.password;

      //this.authService.login(this.loginRequest);
    }
  }
}
