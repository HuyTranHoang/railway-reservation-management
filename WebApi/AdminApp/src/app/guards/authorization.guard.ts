import {Injectable} from '@angular/core';
import {ActivatedRouteSnapshot, Router, RouterStateSnapshot} from '@angular/router';
import {Observable} from 'rxjs';
import {NbGlobalPhysicalPosition, NbToastrService} from '@nebular/theme';
import {AuthService} from '../auth/auth.service';
import {map} from 'rxjs/operators';
import {User} from '../@models/auth/user';

@Injectable({
  providedIn: 'root',
})
export class AuthorizationGuard {

  constructor(private toastrService: NbToastrService,
              private authService: AuthService,
              private router: Router) {
  }

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean> {
    return this.authService.user$.pipe(
      map((user: User | null) => {
        if (user && user.roles.includes('Admin')) {
          return true;
        } else {
          this.showToast('danger', 'Failed', 'You are not authorized to access this page');
          this.router.navigate(['auth'], {queryParams: {returnUrl: state.url}});
          return false;
        }
      }),
    );
  }

  private showToast(type: string, title: string, body: string) {
    const config = {
      status: type,
      destroyByClick: true,
      duration: 4000,
      hasIcon: true,
      position: NbGlobalPhysicalPosition.TOP_RIGHT,
    };
    this.toastrService.show(
      body,
      title,
      config);
  }

}
