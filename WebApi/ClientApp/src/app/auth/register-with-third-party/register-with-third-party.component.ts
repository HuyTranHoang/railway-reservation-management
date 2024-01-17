import { Component, OnInit } from '@angular/core'
import { AuthService } from '../auth.service'
import { ActivatedRoute, Router } from '@angular/router'
import { FormBuilder, FormGroup, Validators } from '@angular/forms'
import { take } from 'rxjs'
import { RegisterWithExternal } from '../../core/models/auth/registerWithExternal'

@Component({
  selector: 'app-register-with-third-party',
  templateUrl: './register-with-third-party.component.html',
  styleUrls: ['../login-and-register.component.scss']
})
export class RegisterWithThirdPartyComponent implements OnInit {
  registerForm: FormGroup = new FormGroup({})
  submitted = false
  provider: string | null = null
  access_token: string | null = null
  userId: string | null = null
  errorMessages: string[] = []

  constructor(private authService: AuthService,
              private fb: FormBuilder,
              private activatedRoute: ActivatedRoute,
              private router: Router) {}

  ngOnInit(): void {
    this.authService.user$.pipe(take(1)).subscribe({
      next: (user) => {
        if (user) this.router.navigateByUrl('/')
        else {
          this.activatedRoute.queryParamMap.subscribe({
            next: (params) => {
              this.provider = this.activatedRoute.snapshot.paramMap.get('provider')
              this.access_token = params.get('accessToken')
              this.userId = params.get('userId')

              if (this.provider && this.access_token && this.userId && (this.provider === 'facebook' || this.provider === 'google')) {
                this.initializeForm()
              } else {
                this.router.navigateByUrl('/auth/register')
              }
            }
          })
        }
      }
    })
  }

  initializeForm() {
    this.registerForm = this.fb.group({
      firstName: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(15)]],
      lastName: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(15)]],
      email: ['', [Validators.required, Validators.pattern('^\\w+@[a-zA-Z_]+?\\.[a-zA-Z]{2,3}$')]],
    })
  }

  onSubmit() {
    this.submitted = true
    this.errorMessages = []

    if (this.registerForm.invalid) {
      return
    }

    const firstName = this.registerForm.value.firstName
    const lastName = this.registerForm.value.lastName
    const email = this.registerForm.value.email

    const model = new RegisterWithExternal(firstName, lastName, email, this.userId!, this.access_token!, this.provider!)
    this.authService.registerWithThirdParty(model).subscribe({
      next: _ => {
        this.router.navigateByUrl('/')
      },
      error: err => {
        console.log(err.errors)
        this.errorMessages = err.errors
      }
    })

  }

}
