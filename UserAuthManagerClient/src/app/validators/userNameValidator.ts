import {AuthService} from '../services/auth.service';
import {AbstractControl, AsyncValidatorFn, ValidationErrors} from '@angular/forms';
import {Observable} from 'rxjs';
import {map} from 'rxjs/operators';

export class UserNameValidator {
  public static userNameExists(authService: AuthService): AsyncValidatorFn {
    return (control: AbstractControl): Observable<ValidationErrors | null> => {
      return authService.checkUserName(control.value).pipe(
        map(res => {
          return res ? null : { busy: true };
        })
      );
    };
  }
}
