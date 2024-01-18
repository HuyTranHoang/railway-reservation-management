import { AfterViewInit, Component, ElementRef, Inject, OnInit, Renderer2, ViewChild } from '@angular/core'
import { FormBuilder, FormGroup, Validators } from '@angular/forms'
import { AuthService } from '../auth.service'
import { ActivatedRoute, Router } from '@angular/router'
import Swal from 'sweetalert2'
import { take } from 'rxjs'
import { LoginWithExternal } from '../../core/models/auth/loginWithExternal'
import { DOCUMENT } from '@angular/common'
import { CredentialResponse } from 'google-one-tap'
import { jwtDecode } from 'jwt-decode'
import { RegisterWithExternal } from '../../core/models/auth/registerWithExternal'

declare const FB: any

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['../login-and-register.component.scss']
})
export class LoginComponent implements OnInit, AfterViewInit {
  @ViewChild('googleButton', { static: true }) googleButton: ElementRef = new ElementRef({})
  loginForm: FormGroup = new FormGroup({})
  submitted = false
  errorMessages: string[] = []
  returnUrl: string = ''
  isResendEmail = false

  constructor(private authService: AuthService,
              private fb: FormBuilder,
              private renderer2: Renderer2,
              @Inject(DOCUMENT) private document: Document,
              private activatedRouter: ActivatedRoute,
              private router: Router) {
    this.authService.user$.pipe(take(1)).subscribe({
      next: (user) => {
        if (user)
          this.router.navigateByUrl('/')
        else {
          const returnUrl = this.activatedRouter.snapshot.queryParams['returnUrl']
          if (returnUrl) {
            this.returnUrl = returnUrl
          }
        }
      }
    })
  }

  ngOnInit(): void {
    this.initializeGoogleButton()
    this.initializeForm()
  }

  ngAfterViewInit() {
    const script = this.renderer2.createElement('script')
    script.src = 'https://accounts.google.com/gsi/client'
    script.async = true
    script.defer = true
    this.renderer2.appendChild(this.document.body, script)
  }


  initializeForm(): void {
    this.loginForm = this.fb.group({
      userName: ['', Validators.required],
      password: ['', Validators.required]
    })
  }

  onSubmit() {
    this.submitted = true
    this.errorMessages = []
    this.isResendEmail = false

    if (this.loginForm.invalid) {
      return
    }

    this.authService.authLogin(this.loginForm.value).subscribe({
      next: (res: any) => {
        Swal.fire({
          position: 'center',
          icon: 'success',
          title: 'You have successfully logged in',
          showConfirmButton: false,
          timer: 1500
        })
        if (this.returnUrl)
          this.router.navigateByUrl(this.returnUrl)
        else
          this.router.navigateByUrl('/')
      },
      error: (err: any) => {
        console.log(err)
        if (err.statusCode === 401 && err.message != 'Invalid username or password') {
          this.isResendEmail = true
        }
      }
    })
  }

  resendEmailConfirmationLink() {
    this.router.navigateByUrl('/auth/send-email/resend-email-confirmation-link')
  }

  loginOrRegisterWithFacebook() {
    FB.login(async (fbResult: any) => {
      if (fbResult.authResponse) {
        // Gọi API để lấy thông tin người dùng, bao gồm email

        let email = '', firstName = '', lastName = '';
        FB.api('/me', { fields: 'first_name,last_name,middle_name,email' }, (response: any) => {
          console.log(">>>>> first Res", response)
          email = response.email;
          firstName = response.first_name;
          lastName = response.last_name + ' ' + response.middle_name;
        });

        const accessToken = fbResult.authResponse.accessToken;
        const userId = fbResult.authResponse.userID;
        const model = new LoginWithExternal(accessToken, userId, 'facebook');

        this.authService.loginWithThirdParty(model).subscribe({
          next: (isUserRegistered: boolean) => {
            console.log(">>>>>", isUserRegistered)
            if (isUserRegistered) {
              // Người dùng đã đăng ký, chuyển hướng đến trang chủ hoặc trang đích
              this.router.navigateByUrl('/');
            } else {
              const model = new RegisterWithExternal(firstName, lastName, email, userId, accessToken, 'facebook')
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
          },
          error: err => {
            console.log(err.errors);
            this.errorMessages = err.errors;
          }
        });
      } else {
        await Swal.fire({
          position: 'center',
          icon: 'error',
          title: 'Facebook login failed',
          text: 'Please try again',
          showConfirmButton: true
        });
      }
    }, { scope: 'email' });
  }

  private initializeGoogleButton() {
    (window as any).onGoogleLibraryLoad = () => {
      // @ts-ignore
      google.accounts.id.initialize({
        client_id: '217119227296-rbmp1am2ba21unl12u3ffuht8e1hgo94.apps.googleusercontent.com',
        callback: this.googleCallBack.bind(this),
        auto_select: false,
        cancel_on_tap_outside: true
      })

      // @ts-ignore
      google.accounts.id.renderButton(this.googleButton.nativeElement, {
        theme: 'outline',
        size: 'medium',
        text: 'continue_with',
        shape: 'rectangular',
        width: 240
      })
    }
  }

  private async googleCallBack(res: CredentialResponse) {
    const decodedToken: any = jwtDecode(res.credential)
    console.log(decodedToken)

    const email = decodedToken.email
    const firstName = decodedToken.given_name
    const lastName = decodedToken.family_name
    const userId = decodedToken.sub
    const accessToken = res.credential

    const model = new LoginWithExternal(accessToken, userId, 'google')
    this.authService.loginWithThirdParty(model).subscribe({
      next: (isUserRegistered: boolean) => {
        console.log('>>>>>', isUserRegistered)
        if (isUserRegistered) {
          // Người dùng đã đăng ký, chuyển hướng đến trang chủ hoặc trang đích
          this.router.navigateByUrl('/')
        } else {
          const model = new RegisterWithExternal(firstName, lastName, email, userId, accessToken, 'google')
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
      },
      error: err => {
        console.log(err.errors)
        this.errorMessages = err.errors
      }
    })

  }

}
