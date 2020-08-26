import { Component, OnInit } from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {AuthService} from '../../services/auth.service';
import {UserRegister} from '../../models/userRegister';
import {UserNameValidator} from '../../validators/userNameValidator';
import {MustMatch} from '../../validators/mustMatchValidator';
import {Router} from '@angular/router';

import * as AOS from 'aos';
import {DestroyService} from '../../services/destroy.service';
import {takeUntil} from 'rxjs/operators';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router,
    private destroy$: DestroyService
    ) {

    this.createForm();
  }

  user: UserRegister;
  registerForm: FormGroup;
  errors: any;
  formSent = false;

  formErrors = {
    userName: '',
    email: '',
    password: '',
    reEnterPassword: ''
  };

  validationsMessages = {
    userName: {
      required: 'User name is required!',
      busy: 'This username is already taken!'
    },
    email: {
      required: 'Email is required!',
      email: 'Invalid email!'
    },
    password: {
      required: 'Password is required!'
    },
    reEnterPassword: {
      required: 'Re-enter password is required!',
      mustMatch: 'Passwords must match'
    }
  };

  createForm(){
    this.registerForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      userName: ['', [Validators.required], [UserNameValidator.userNameExists(this.authService)]],
      password: ['', [Validators.required]],
      reEnterPassword: ['', [Validators.required]]
    }, {
      validator: MustMatch('password', 'reEnterPassword')
    });

    this.registerForm.valueChanges
      .subscribe(data => this.onValueChanged(data));

    this.onValueChanged(); // (re)set validation messages now
  }

  onValueChanged(data?: any) {
    if (!this.registerForm) { return; }

    const form = this.registerForm;

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

  onSubmit() {
    this.user = this.registerForm.value;
    this.formSent = true;

    this.authService.register(this.user)
      .pipe(takeUntil(this.destroy$))
      .subscribe(x => {
        this.registerForm.reset();
        this.formSent = false;

        this.autoLogin();
    },
        error => this.errors = error);
  }

  autoLogin(){
    this.authService.login({
      email: this.user.email,
      password: this.user.password
    })
      .pipe(takeUntil(this.destroy$))
      .subscribe(x => {
        this.router.navigate(['/users']);
    });
  }

}
