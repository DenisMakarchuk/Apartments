import { Component, OnInit } from '@angular/core';
import { UserService } from 'src/app/core/nswag.generated.service';
import * as jwt_decode from 'jwt-decode';


@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.scss']
})
export class MenuComponent implements OnInit {

  constructor(private authService: UserService) { }

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
  
}
