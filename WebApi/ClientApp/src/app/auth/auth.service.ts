import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http'
import { environment } from '../../environments/environment.development'
import { Register } from '../models/register'
import { Login } from '../models/login'
import { User } from '../models/user'
import { map, ReplaySubject } from 'rxjs'

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  baseUrl = environment.apiUrl
  private userSource = new ReplaySubject<User | null>(1)
  user$ = this.userSource.asObservable()

  constructor(private http: HttpClient) { }

  register(model: Register) {
    return this.http.post(this.baseUrl + 'account/register', model)
  }

  login(model: Login) {
    return this.http.post<User>(this.baseUrl + 'account/login', model).pipe(
      map((user: User) => {
        if (user) {
          this.setUser(user)
          return user
        }

        return null
      })
    )
  }

  logout() {
    localStorage.removeItem(environment.userKey)
    this.userSource.next(null)
  }

  private setUser(user: User) {
    localStorage.setItem(environment.userKey, JSON.stringify(user))
    this.userSource.next(user)
  }
}
