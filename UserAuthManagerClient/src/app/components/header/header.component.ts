import { Component, OnInit } from '@angular/core';
import {ACCESS_TOKEN_KEY, AuthService} from '../../services/auth.service';
import {JwtHelperService} from '@auth0/angular-jwt';

import * as AOS from 'aos';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {

  constructor(
    private authService: AuthService,
    private helper: JwtHelperService
  ) { }

  ngOnInit(): void {
    AOS.init();
  }

  public get isLoggedIn(): boolean {
    return this.authService.isAuthenticated();
  }

  userEmail(): string{
    if (this.isLoggedIn){
      const decodedToken = this.helper.decodeToken(localStorage.getItem(ACCESS_TOKEN_KEY));
      return decodedToken.email;
    }
  }

  logout(){
    this.authService.logout();
  }

}
