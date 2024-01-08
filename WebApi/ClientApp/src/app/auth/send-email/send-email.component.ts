import { Component, OnInit } from '@angular/core'
import { AuthService } from '../auth.service'
import { ActivatedRoute, Router } from '@angular/router'
import { FormBuilder, FormGroup, Validators } from '@angular/forms'
import Swal from 'sweetalert2'
import { take } from 'rxjs'


@Component({
  selector: 'app-send-email',
  templateUrl: './send-email.component.html',
  styleUrls: ['./send-email.component.scss', '../login-and-register.component.scss']
})
export class SendEmailComponent implements OnInit{
  emailForm: FormGroup = new FormGroup({})
  submitted = false
  mode: string = ''
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
          const mode = this.activatedRoute.snapshot.paramMap.get('mode')
          if (mode) {
            this.mode = mode
            this.initializeForm()
          }
        }
      }

    })
  }

  initializeForm() {
    this.emailForm = this.fb.group({
      email: ['', [Validators.required, Validators.pattern('^\\w+@[a-zA-Z_]+?\\.[a-zA-Z]{2,3}$')]],
    })
  }

  onSubmit() {
    this.submitted = true
    this.errorMessages = []

    if (this.emailForm.invalid) {
      return
    }

    if (!this.mode) {
      return
    }

    if (this.mode.includes('resend-email-confirmation')) {
      this.authService.resendEmailConfirmation(this.emailForm.get('email')?.value).subscribe({
        next: (res: any) => {
          Swal.fire({
            title: 'Email Sent!',
            text: res.value.message,
            icon: 'success',
            confirmButtonText: 'Ok'
          })

          this.router.navigateByUrl('/auth/login')
        },
        error: (err: any) => {
          this.errorMessages = err.errors
        }
      })
    }


  }

}
