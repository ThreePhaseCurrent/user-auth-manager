import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { MatInputModule } from '@angular/material/input';
import { MatCardModule } from '@angular/material/card';
import {MatButtonModule} from '@angular/material/button';
import {HttpClientModule} from '@angular/common/http';

import { AppRoutingModule } from './app-routing/app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { LoginComponent } from './components/login/login.component';
import { UserListComponent } from './components/user-list/user-list.component';
import { UserDetailComponent } from './components/user-detail/user-detail.component';
import { RegisterComponent } from './components/register/register.component';
import { HeaderComponent } from './components/header/header.component';
import { HomeComponent } from './components/home/home.component';
import {ACCESS_TOKEN_KEY, AuthService} from './services/auth.service';
import {baseURL} from './shared/baseURL';
import {HTTP_INTERCEPTORS} from '@angular/common/http';
import {HttpErrorsInterceptorService} from './interceptors/http-errors-interceptor.service';
import {JwtModule} from '@auth0/angular-jwt';
import {environment} from 'src/environments/environment';
import {ReactiveFormsModule} from '@angular/forms';
import {AuthGuard} from './app-routing/auth-guard';

export function tokenGetter(){
  return localStorage.getItem(ACCESS_TOKEN_KEY);
}

// @ts-ignore
// @ts-ignore
// @ts-ignore
// @ts-ignore
@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    UserListComponent,
    UserDetailComponent,
    RegisterComponent,
    HeaderComponent,
    HomeComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MatInputModule,
    MatCardModule,
    MatButtonModule,
    HttpClientModule,

    JwtModule.forRoot({
      config: {
        tokenGetter,
        allowedDomains: environment.tokenWhitelistedDomains
      }
    }),
    ReactiveFormsModule
  ],
  providers: [AuthService, AuthGuard, {provide: 'baseURL', useValue: baseURL},
    { provide: HTTP_INTERCEPTORS, useClass: HttpErrorsInterceptorService, multi: true }],
  bootstrap: [AppComponent]
})
export class AppModule { }
