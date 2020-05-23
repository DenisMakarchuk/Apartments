import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { UserService } from 'src/app/services/nswag.generated.service';

@Component({
  selector: 'app-email-confirmation',
  templateUrl: './email-confirmation.component.html',
  styleUrls: ['./email-confirmation.component.scss']
})
export class EmailConfirmationComponent implements OnInit {

  spinning = true;
  errorMessage: string;

  userId: string;
  token: string;

  isConfirmed = false;

  constructor(
    private route: ActivatedRoute,
    private userService: UserService
    ) { 
      this.userId = route.snapshot.queryParamMap.get('userId');
      this.token = route.snapshot.queryParamMap.get('token');

      this.userService.confirmEmail(this.userId, this.token)
      .subscribe(() => {
        this.errorMessage = null;
        this.spinning = false;

        this.isConfirmed = true;
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

  ngOnInit(): void {
  }
}
