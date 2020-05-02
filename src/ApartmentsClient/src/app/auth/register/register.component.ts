import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, FormControl } from '@angular/forms';

import { UserService } from 'src/app/core/nswag.generated.service';
import { UserViewModel } from 'src/app/core/nswag.generated.service';
import { UserRegistrationRequest } from 'src/app/core/nswag.generated.service';

import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {

  registerForm: FormGroup;

  registerRequest: UserRegistrationRequest;
  user: UserViewModel;
  currentUser = {};

  email: string;
  password: string;

  constructor( 
    private registerService: UserService,
    public fb: FormBuilder,
    public router: Router
    ) { 
      this.registerForm = this.fb.group({
        email: '',
        password: ''
      });
    }

  ngOnInit(): void {
  }

  register() {
    if (this.registerForm.valid) {
      this.registerService.register(this.registerForm.value)
      .subscribe(user => this.user = user);
  
      if (this.user != null) {
        localStorage.setItem('access_token', this.user.token)
        this.currentUser = this.user;
  
        this.registerForm.reset();
        this.router.navigate(['/profile']);
      }
    }
  }


}
