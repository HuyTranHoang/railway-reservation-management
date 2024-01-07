import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http'
import { environment } from '../../environments/environment.development'
import { Register } from '../models/register'
import { Login } from '../models/login'

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  baseUrl = environment.apiUrl

  constructor(private http: HttpClient) { }

  register(model: Register) {
    return this.http.post(this.baseUrl + 'account/register', model)
  }

  login(model: Login) {
    return this.http.post(this.baseUrl + 'account/login', model)
  }
}
