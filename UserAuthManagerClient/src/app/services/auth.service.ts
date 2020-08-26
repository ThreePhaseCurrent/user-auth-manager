import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {ShortUser} from '../models/shortUser';
import {baseURL} from '../shared/baseURL';
import {UserLogin} from '../models/userLogin';
import {tap} from 'rxjs/operators';
import {Token} from '../models/token';
import {JwtHelperService} from '@auth0/angular-jwt';
import {Router} from '@angular/router';
import {UserRegister} from '../models/userRegister';

export const ACCESS_TOKEN_KEY = 'access_token';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(
    private http: HttpClient,
    private jwtHelper: JwtHelperService,
    private router: Router
  ) { }

  isAuthenticated(): boolean {
    const token = localStorage.getItem(ACCESS_TOKEN_KEY);
    return token && !this.jwtHelper.isTokenExpired(token);
  }

  getUsers(): Observable<ShortUser[]> {
    return this.http.get<ShortUser[]>(`${baseURL}users`);
  }

  login(user: UserLogin): Observable<Token> {
    return this.http.post<Token>(`${baseURL}login`, {
      email: user.email,
      password: user.password
    }).pipe(tap(data => {
      localStorage.setItem(ACCESS_TOKEN_KEY, data.accessToken);
    }));
  }

  register(user: UserRegister): Observable<any> {
    return this.http.post(`${baseURL}register`, {
      email: user.email,
      userName: user.userName,
      password: user.password
    });
  }

  checkUserName(userName: string): Observable<boolean>{
    if (userName === '') { return null; }

    return this.http.get<boolean>(`${baseURL}username-check/` + userName);
  }

  logout() {
    localStorage.removeItem(ACCESS_TOKEN_KEY);

    this.router.navigate(['']);
  }
}
