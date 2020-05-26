import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

import { UserService } from 'src/app/services/nswag.generated.service';

import { Router } from '@angular/router';

@Component({
  selector: 'app-forgot',
  templateUrl: './forgot.component.html',
  styleUrls: ['./forgot.component.scss']
})
export class ForgotComponent implements OnInit {

  spinning = false;
  errorMessage: string;

  forgotForm: FormGroup;

  isOk = false;

  constructor(    
    private service: UserService,
    public fb: FormBuilder,
    public router: Router
  ) { 
    this.forgotForm = this.fb.group({
      callBackUrl: 'http://localhost:4200/reset-password',
      logInNameOrEmail: ['', [Validators.required]]
    });
  }

  ngOnInit(): void {
  }

  get _logInNameOrEmail(){
    return this.forgotForm.get('logInNameOrEmail');
  }

  sendForgotMessage(){
    this.errorMessage = null;
    this.spinning = true;

    if (this.forgotForm.valid) {
      this.service.forgotPassword(this.forgotForm.value)
      .subscribe(() => 
      {
        this.spinning = false;
        this.isOk = true;
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
