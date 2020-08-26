import { Component, OnInit } from '@angular/core';
import {UserLogin} from '../../models/userLogin';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {AuthService} from '../../services/auth.service';
import {Router} from '@angular/router';

import * as AOS from 'aos';
import {DestroyService} from '../../services/destroy.service';
import {takeUntil} from 'rxjs/operators';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  user: UserLogin;
  loginForm: FormGroup;
  hasError: any;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router,
    private destroy$: DestroyService
  ) {

    this.createForm();
  }

  formErrors = {
    email: '',
    password: ''
  };

  validationsMessages = {
    email: {
      required: 'Email is required!',
      email: 'Invalid email!'
    },
    password: {
      required: 'Password is required!'
    }
  };

  createForm(){
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required]]
    });

    this.loginForm.valueChanges
      .subscribe(data => this.onValueChanged(data));

    this.onValueChanged(); // (re)set validation messages now
  }

  onValueChanged(data?: any) {
    if (!this.loginForm) { return; }

    const form = this.loginForm;

    for (const field in this.formErrors) {

      if (this.formErrors.hasOwnProperty(field))
      {
        this.formErrors[field] = '';
        const control = form.get(field);

        if (control && !control.valid)
        {
          const messages = this.validationsMessages[field];

          for (const key in control.errors)
          {
            if (control.errors.hasOwnProperty(key))
            {
              this.formErrors[field] += messages[key] + ' ';
            }
          }
        }
      }
    }
  }

  ngOnInit(): void {
    if (this.authService.isAuthenticated()){
      this.router.navigate(['/users']);
    }

    AOS.init();
  }

  onSubmit(){
    this.user = this.loginForm.value;

    this.authService.login(this.user)
      .pipe(takeUntil(this.destroy$))
      .subscribe(x => {
        this.router.navigate(['/users']);
    },
      error => this.hasError = error);
  }

}
