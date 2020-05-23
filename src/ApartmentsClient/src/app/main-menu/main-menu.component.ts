import { Component, OnInit } from '@angular/core';
import { LoggedService } from 'src/app/services/logged.service';
import * as jwt_decode from 'jwt-decode';

@Component({
  selector: 'app-main-menu',
  templateUrl: './main-menu.component.html',
  styleUrls: ['./main-menu.component.scss']
})
export class MainMenuComponent implements OnInit {

  constructor(private authService: LoggedService) { }

  ngOnInit(): void {
  }

  get isLoggedIn(): boolean {
    return this.authService.isLoggedIn;
  }

  get isAdmin(){
      var token = this.authService.getToken();
        
      var decodedoken = jwt_decode(token);
      var currentRole = decodedoken['role'];
  
      if (currentRole.includes('Admin')) {
        return true;
      }
      return false;
  }

  get currentEmail(): string{
    var token = this.authService.getToken();
        
      var decodedoken = jwt_decode(token);
      return decodedoken['email'];
  }

}
