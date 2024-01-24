import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, AbstractControl, ValidationErrors } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { take } from 'rxjs/operators';
import { AuthService } from '../auth.service';
import { NbToastrService, NbGlobalPhysicalPosition } from '@nebular/theme';
import { ResetPassword } from '../../@models/auth/resetPassword';

@Component({
  selector: 'ngx-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.scss']
})
export class ResetPasswordComponent implements OnInit {

  resetPasswordForm: FormGroup = new FormGroup({})
  token: string | null = null
  email: string | null = null
  submitted = false
  errorMessages: string[] = []

  constructor(private authService: AuthService,
              private router: Router,
              private activatedRoute: ActivatedRoute,
              private fb: FormBuilder,
              private toastrService: NbToastrService,) {}

  ngOnInit(): void {
    this.authService.user$.pipe(take(1)).subscribe({
      next: (user) => {
        if (user) {
          this.router.navigateByUrl('/')
        } else {
          this.activatedRoute.queryParamMap.subscribe({
            next: (params) => {
              this.token = params.get('token')
              this.email = params.get('email')

              if (this.token && this.email) {
                this.initializeForm();
              }
            }
          })
        }
      }
    })
  }

  initializeForm() {
    this.resetPasswordForm = this.fb.group({
      password: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(15)]],
      confirmPassword: ['', [Validators.required, this.passwordMatchValidator.bind(this)]]
    })
  }

  passwordMatchValidator(control: AbstractControl): ValidationErrors | null {
    const password = this.resetPasswordForm.get('password')?.value;
    if (control.value !== password) {
      return { 'mismatch': true };
    }
    return null;
  }

  onSubmit() {
    this.submitted = true
    this.errorMessages = []

    if (this.resetPasswordForm.invalid) {
      return
    }

    const resetPasswordData: ResetPassword = {
      token: this.token!,
      email : this.email!,
      newPassword: this.resetPasswordForm.value.password
    }

    this.authService.resetPassword(resetPasswordData).subscribe({
      next: (res: any) => {
        this.showToast('success', 'Success', 'Reset password successfully');
        this.router.navigate(['auth/login'])
        this.errorMessages = []
      },
      error: (err) => {
        this.showToast('danger', 'Failed', 'Reset password failed!');
        this.errorMessages = err.errors
      }
    })

  }

  private showToast(type: string, title: string, body: string) {
    const config = {
      status: type,
      destroyByClick: true,
      duration: 2000,
      hasIcon: true,
      position: NbGlobalPhysicalPosition.TOP_RIGHT,
    };
    this.toastrService.show(
      body,
      title,
      config);
  }

}
