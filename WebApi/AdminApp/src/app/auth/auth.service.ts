import {Injectable} from '@angular/core';
import {environment} from '../../environments/environment';
import {of, ReplaySubject} from 'rxjs';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import {Router} from '@angular/router';
import {Register} from '../@models/auth/register';
import {RegisterWithExternal} from '../@models/auth/registerWithExternal';
import {map} from 'rxjs/operators';
import {Login} from '../@models/auth/login';
import {LoginWithExternal} from '../@models/auth/loginWithExternal';
import {ConfirmEmail} from '../@models/auth/confirmEmail';
import {ResetPassword} from '../@models/auth/resetPassword';
import {ChangePassword} from '../@models/auth/changePassword';
import {UpdateProfile} from '../@models/auth/updateProfile';
import {User} from '../@models/auth/user';

@Injectable({
  providedIn: 'root',
})

export class AuthService {
  baseUrl = environment.apiUrl;
  private userSource = new ReplaySubject<User | null>(1);
  user$ = this.userSource.asObservable();

  constructor(private http: HttpClient, private router: Router) {
  }

  register(model: Register) {
    return this.http.post(this.baseUrl + '/account/register', model);
  }

  registerWithThirdParty(model: RegisterWithExternal) {
    return this.http.post<User>(this.baseUrl + '/account/register-with-third-party', model).pipe(
      map((user: User) => {
        if (user) {
          this.setUser(user);
          return user;
        }

        return null;
      }),
    );
  }

  authLogin(model: Login) {
    return this.http.post<User>(this.baseUrl + '/account/login', model).pipe(
      map((user: User) => {
        if (user && user.roles.includes('Admin')) {
          this.setUser(user);
          return user;
        } else if (user && !user.roles.includes('Admin')) {
          return 'NotAdmin';
        }
        return null;
      }),
    );
  }

  refreshUser(jwt: string | null) {
    if (jwt === null) {
      this.userSource.next(null);
      return of(undefined);
    }

    let headers = new HttpHeaders();
    headers = headers.set('Authorization', `Bearer ${jwt}`);

    return this.http.get<User>(this.baseUrl + '/account/refresh-user-token', {headers}).pipe(
      map((user: User) => {
        if (user) {
          this.setUser(user);
        }
      }),
    );
  }

  loginWithThirdParty(model: LoginWithExternal) {
    return this.http.post<{
      isUserRegistered: boolean,
      user: User,
    }>(this.baseUrl + '/account/login-with-third-party', model).pipe(
      map((res: { isUserRegistered: boolean, user: User }) => {
        if (res.isUserRegistered) {
          this.setUser(res.user);
          return res.isUserRegistered;
        }
        return res.isUserRegistered;
      }),
    );
  }

  logout() {
    localStorage.removeItem(environment.userKey);
    this.userSource.next(null);
    this.router.navigateByUrl('/');
  }

  getJwtToken() {
    const key = localStorage.getItem(environment.userKey);
    if (key) {
      const user: User = JSON.parse(key);
      return user.jwt;
    }

    return null;
  }

  confirmEmail(model: ConfirmEmail) {
    return this.http.put(this.baseUrl + '/account/confirm-email', model);
  }

  resendEmailConfirmation(email: string) {
    return this.http.post(this.baseUrl + `/account/resend-email-confirmation-link/${email}`, {});
  }

  forgotPassword(email: string) {
    return this.http.post(this.baseUrl + `/account/forgot-password/${email}`, {});
  }

  forgotPasswordAdmin(email: string) {
    return this.http.post(this.baseUrl + `/account/forgot-password-admin/${email}`, {});
  }

  resetPassword(model: ResetPassword) {
    return this.http.put(this.baseUrl + '/account/reset-password', model);
  }

  changePassword(model: ChangePassword) {
    return this.http.put(this.baseUrl + '/account/change-password', model);
  }

  updateProfile(model: UpdateProfile) {
    return this.http.put<User>(this.baseUrl + '/account/update-profile', model).pipe(
      map((user: User) => {
        if (user) {
          this.setUser(user);
        }
      }),
    );
  }

  private setUser(user: User) {
    localStorage.setItem(environment.userKey, JSON.stringify(user));
    this.userSource.next(user);
  }
}
