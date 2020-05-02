import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { UserService } from 'src/app/core/nswag.generated.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(
    private router: Router,
    private authService: UserService
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
