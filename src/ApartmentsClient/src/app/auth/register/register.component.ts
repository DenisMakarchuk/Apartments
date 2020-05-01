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

  constructor( 
    private service: UserService,
    public fb: FormBuilder,
    public router: Router
    ) { 
      this.registerForm = this.fb.group({
        name: [''],
        password: ['']
      });
    }

  ngOnInit(): void {
  }

  register() {
    if (this.registerForm.valid) {
      const data = this.registerForm.value;

      this.registerRequest.email = data.name;
      this.registerRequest.password = data.password;

      this.service.register(this.registerRequest)
        .subscribe(user => this.user = user);

        this.router.navigate(['login']);
        this.registerForm.reset();
    }
  }
}
