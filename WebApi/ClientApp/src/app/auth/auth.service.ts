import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http'
import { environment } from '../../environments/environment.development'
import { Register } from '../core/models/register'
import { Login } from '../core/models/login'
import { User } from '../core/models/user'
import { map, of, ReplaySubject } from 'rxjs'
import { Router } from '@angular/router'

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  baseUrl = environment.apiUrl
  private userSource = new ReplaySubject<User | null>(1)
  user$ = this.userSource.asObservable()

  constructor(private http: HttpClient, private router: Router) { }

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

  refreshUser(jwt: string | null) {
    if (jwt === null) {
      this.userSource.next(null)
      return of(undefined)
    }

    let headers = new HttpHeaders()
    headers = headers.set('Authorization', `Bearer ${jwt}`)

    return this.http.get<User>(this.baseUrl + 'account/refresh-user-token', { headers }).pipe(
      map((user: User) => {
        if (user) {
          this.setUser(user)
        }
      })
    )
  }

  logout() {
    localStorage.removeItem(environment.userKey)
    this.userSource.next(null)
    this.router.navigateByUrl('/')
  }

  getJwtToken() {
    const key = localStorage.getItem(environment.userKey)
    if (key) {
      const user: User = JSON.parse(key)
      return user.jwt
    }

    return null
  }

  private setUser(user: User) {
    localStorage.setItem(environment.userKey, JSON.stringify(user))
    this.userSource.next(user)
  }
}
