import Swal from 'sweetalert2'
import {catchError, Observable, throwError} from 'rxjs'
import {NavigationExtras, Router} from '@angular/router'
import {Injectable} from '@angular/core'
import {HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest} from '@angular/common/http'


@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  constructor(private router: Router) {
  }

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      catchError((error: HttpErrorResponse) => {
        switch (error.status) {
          case 400:
            if (error.error.errors) {
              throw error.error
            } else {
              Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: error.error.message
              })
            }
            break
          case 401:
            Swal.fire({
              icon: 'error',
              title: 'Oops...',
              text: error.error.message
            })
            throw error.error
          case 404:
            this.router.navigateByUrl('/not-found')
            break
          case 500:
            const navigationExtras: NavigationExtras = {state: {error: error.error}}
            this.router.navigateByUrl('/server-error', navigationExtras)
            break
          default:
            break
        }

        return throwError(() => new Error(error.message))
      })
    )
  }
}
