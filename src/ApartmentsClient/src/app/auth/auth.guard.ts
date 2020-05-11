import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { LoggedService } from '../services/logged.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(
    private router: Router,
    private authService: LoggedService
  ) { }

  canActivate(): boolean {
    if (this.authService.isLoggedIn) {
      return true;
    }else{
      window.alert("Access not allowed!");
      this.router.navigate(['/login']);
    }
  }
}
