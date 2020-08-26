import { CanActivate, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { Inject, Injectable } from '@angular/core';

@Injectable()
export class AuthGuard implements CanActivate {

  constructor(private authService: AuthService,
              private router: Router) {}

  canActivate(){
    if (this.authService.isAuthenticated()) {
      return true;
    }

    this.router.navigate(['']);
    return false;
  }
}
