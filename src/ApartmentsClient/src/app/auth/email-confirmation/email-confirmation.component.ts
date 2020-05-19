import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { Subscription } from 'rxjs';
import { UserService } from 'src/app/services/nswag.generated.service';

@Component({
  selector: 'app-email-confirmation',
  templateUrl: './email-confirmation.component.html',
  styleUrls: ['./email-confirmation.component.scss']
})
export class EmailConfirmationComponent implements OnInit {

  userId: string;
  token: string;

  isConfirmed = false;

  private querySubscription: Subscription;

  constructor(
    private route: ActivatedRoute,
    private userService: UserService
    ) { 
      
      this.userId = route.snapshot.queryParamMap.get('userId');
      this.token = route.snapshot.queryParamMap.get('token');

      this.userService.confirmEmail(this.userId, this.token)
      .subscribe(() => {
        this.isConfirmed = true;
      });
  }

  ngOnInit(): void {
  }
}
