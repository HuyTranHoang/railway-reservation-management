import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {AuthService} from '../auth.service';
import {ActivatedRoute, Router} from '@angular/router';
import {NbGlobalPhysicalPosition, NbToastrService} from '@nebular/theme';
import {take} from 'rxjs/operators';
import {User} from '../../@models/auth/user';

@Component({
  selector: 'ngx-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent implements OnInit {

  loginForm: FormGroup = new FormGroup({});
  submitted = false;
  errorMessages: string[] = [];
  returnUrl: string = '';

  constructor(private authService: AuthService,
              private fb: FormBuilder,
              private toastrService: NbToastrService,
              private activatedRouter: ActivatedRoute,
              private router: Router) {
    this.authService.user$.pipe(take(1)).subscribe({
      next: (user) => {
        if (user)
          this.router.navigateByUrl('/managements/dashboard');
        else {
          const returnUrl = this.activatedRouter.snapshot.queryParams['returnUrl'];
          if (returnUrl) {
            this.returnUrl = returnUrl;
          }
        }
      },
    });
  }

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm(): void {
    this.loginForm = this.fb.group({
      userName: ['', Validators.required],
      password: ['', Validators.required],
    });
  }

  onSubmit() {
    this.submitted = true;
    this.errorMessages = [];

    if (this.loginForm.invalid) {
      return;
    }

    this.authService.authLogin(this.loginForm.value).subscribe({
      next: (res: any) => {

        if (res === 'NotAdmin') {
          this.showToast('danger', 'Failed', 'You are not authorized to access this page');
          return;
        }

        if (res.roles.includes('Admin'))
          this.showToast('success', 'Success', 'Login successfully!');

        if (this.returnUrl)
          this.router.navigateByUrl(this.returnUrl);
        else
          this.router.navigateByUrl('/managements/dashboard');
      },
      error: (err: any) => {
        this.showToast('danger', 'Failed', 'Wrong username or password!');
      },
    });
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
