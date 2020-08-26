import {Routes} from '@angular/router';
import {LoginComponent} from '../components/login/login.component';
import {HomeComponent} from '../components/home/home.component';
import {RegisterComponent} from '../components/register/register.component';
import {UserListComponent} from '../components/user-list/user-list.component';
import {AuthGuard} from './auth-guard';

export const routes: Routes = [
  {path: '', component: HomeComponent},
  {path: 'login', component: LoginComponent},
  {path: 'register', component: RegisterComponent},
  { path: 'users', component: UserListComponent, canActivate: [AuthGuard]}
];
