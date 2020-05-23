import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { UserService } from 'src/app/services/nswag.generated.service';
import { FormGroup, FormBuilder, FormControl } from '@angular/forms';

@Component({
  selector: 'app-reset',
  templateUrl: './reset.component.html',
  styleUrls: ['./reset.component.scss']
})
export class ResetComponent implements OnInit {

  spinning = false;
  errorMessage: string;

  userId: string;
  token: string;

  resetForm: FormGroup;

  isReseted = false;

  constructor(
    private route: ActivatedRoute,
    public fb: FormBuilder,
    private userService: UserService
  ) { 
    this.userId = route.snapshot.queryParamMap.get('userId');
    this.token = route.snapshot.queryParamMap.get('token');

    this.resetForm = this.fb.group({
      userId: this.userId,
      token: this.token,
      password: '',
      callBackUrl: 'http://localhost:4200/login'
    });
  }

  ngOnInit(): void {
  }

  reset(){
    this.errorMessage = null;
    this.spinning = true;

    if (this.resetForm.valid) {
      this.userService.resetPassword(this.resetForm.value)
      .subscribe(() => 
      {
        this.spinning = false;
        this.isReseted = true;
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
