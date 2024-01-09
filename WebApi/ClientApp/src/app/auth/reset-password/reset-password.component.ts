import { Component, OnInit } from '@angular/core'
import { AuthService } from '../auth.service'
import { ActivatedRoute, Router } from '@angular/router'
import { AbstractControl, FormBuilder, FormGroup, ValidationErrors, Validators } from '@angular/forms'
import { take } from 'rxjs'
import { ResetPassword } from '../../core/models/auth/resetPassword'
import Swal from 'sweetalert2'

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['../login-and-register.component.scss']
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
              private fb: FormBuilder) {}

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
                this.initializeForm(this.email)
              }
            }
          })
        }
      }
    })
  }

  initializeForm(username: string) {
    this.resetPasswordForm = this.fb.group({
      email: [{value: username, disabled: true}],
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
      email: this.email!,
      newPassword: this.resetPasswordForm.value.password
    }

    this.authService.resetPassword(resetPasswordData).subscribe({
      next: (res: any) => {
        Swal.fire({
          position: 'center',
          icon: 'success',
          title: res.value.title,
          text: res.value.message,
          showConfirmButton: true
        })

        this.router.navigate(['auth/login'])
        this.errorMessages = []
      },
      error: (err) => {
        this.errorMessages = err.errors
      }
    })

  }

}
