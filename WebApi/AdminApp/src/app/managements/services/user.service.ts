import {Injectable} from '@angular/core';
import {environment} from '../../../environments/environment';
import {HttpClient} from '@angular/common/http';
import {ApplicationUser} from '../../@models/ApplicationUser';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {
  }

  getUserByEmail(email: string) {
    return this.http.get<ApplicationUser>(this.baseUrl + '/users/' + email);
  }
}
