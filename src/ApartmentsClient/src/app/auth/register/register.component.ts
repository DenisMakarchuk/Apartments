import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

import { UserService } from 'src/app/services/nswag.generated.service';

import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {

  spinning = false;
  errorMessage: string;

  registerForm: FormGroup;
  confirmPasswordForm: FormGroup;

  isregistered = false;

  constructor( 
    private registerService: UserService,
    public fb: FormBuilder,
    public router: Router
  ) { 
    this.registerForm = this.fb.group({
      userName: ['', [Validators.required]],
      nickName: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(8)]],
      callBackUrl: 'http://localhost:4200/confirmation'
    });

    this.confirmPasswordForm = this.fb.group({
      confirmPassword: ''
    })
  }

  ngOnInit(): void {
  }

  get _userName(){
    return this.registerForm.get('userName');
  } 
  
  get _nickName(){
    return this.registerForm.get('nickName');
  }  
  
  get _email(){
    return this.registerForm.get('email');
  }  
  
  get _password(){
    return this.registerForm.get('password');
  }

  get _confirmPassword(){
    return this.confirmPasswordForm.get('confirmPassword');
  }

  register() {
    this.errorMessage = null;
    this.spinning = true;

    if (this.registerForm.valid && 
      this.registerForm.value.password === this.confirmPasswordForm.value.confirmPassword) {

      this.registerService.register(this.registerForm.value)
      .subscribe(() => 
      {
        this.spinning = false;
        this.isregistered = true;
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
