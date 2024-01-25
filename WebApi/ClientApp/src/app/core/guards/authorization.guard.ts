import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router'
import { map, Observable } from 'rxjs'
import { AuthService } from '../../auth/auth.service'
import { User } from '../models/auth/user'
import Swal from 'sweetalert2'

@Injectable({
  providedIn: 'root'
})
export class AuthorizationGuard {
  constructor(private authService: AuthService, private router: Router) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean> {
    return this.authService.user$.pipe(
      map((user : User | null) => {
        if (user)
          return true
        else {
          Swal.fire({
            position: 'center',
            icon: 'warning',
            title: 'Oops...',
            text: 'You need to login to continue!',
            showConfirmButton: false,
            timer: 1500
          })

          this.router.navigate(['auth/login'], {queryParams: {returnUrl: state.url}})
          return false
        }

      })
    )
  }

}
