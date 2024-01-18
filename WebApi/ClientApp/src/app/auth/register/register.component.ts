import { Component, ElementRef, Inject, OnInit, Renderer2, ViewChild } from '@angular/core'
import { AuthService } from '../auth.service'
import { AbstractControl, FormBuilder, FormGroup, ValidationErrors, Validators } from '@angular/forms'
import Swal from 'sweetalert2'
import { Router } from '@angular/router'
import { take } from 'rxjs'
import { LoginWithExternal } from '../../core/models/auth/loginWithExternal'
import { RegisterWithExternal } from '../../core/models/auth/registerWithExternal'
import { CredentialResponse } from 'google-one-tap'
import {jwtDecode} from 'jwt-decode'
import { DOCUMENT } from '@angular/common'

declare const FB: any

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['../login-and-register.component.scss']
})
export class RegisterComponent implements OnInit {
  @ViewChild('googleButton', { static: true }) googleButton: ElementRef = new ElementRef({})

  registerForm: FormGroup = new FormGroup({})
  submitted = false
  errorMessages: string[] = []

  constructor(private authService: AuthService,
              private renderer2: Renderer2,
              @Inject(DOCUMENT) private document: Document,
              private fb: FormBuilder,
              private router: Router) {
    this.authService.user$.pipe(take(1)).subscribe({
      next: (user) => {
        if (user) this.router.navigateByUrl('/')
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
    this.registerForm = this.fb.group({
      firstName: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(15)]],
      lastName: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(15)]],
      email: ['', [Validators.required, Validators.pattern('^\\w+@[a-zA-Z_]+?\\.[a-zA-Z]{2,3}$')]],
      password: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(15)]],
      confirmPassword: ['', [Validators.required, this.passwordMatchValidator.bind(this)]]
    })
  }

  passwordMatchValidator(control: AbstractControl): ValidationErrors | null {
    const password = this.registerForm.get('password')?.value
    if (control.value !== password) {
      return { 'mismatch': true }
    }
    return null
  }

  onSubmit() {
    this.submitted = true
    this.errorMessages = []

    if (this.registerForm.invalid) {
      return
    }

    const formData = {
      firstName: this.registerForm.value.firstName,
      lastName: this.registerForm.value.lastName,
      email: this.registerForm.value.email,
      password: this.registerForm.value.password
    }

    this.authService.register(formData).subscribe({
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
        this.submitted = false
        this.registerForm.reset()
      },
      error: (err) => {
        console.log(err.errors)
        this.errorMessages = err.errors
      }
    })
  }

  // registerWithFacebook() {
  //   FB.login(async (fbResult: any) => {
  //     if (fbResult.authResponse) {
  //       console.log(fbResult)
  //       const accessToken = fbResult.authResponse.accessToken
  //       const userId = fbResult.authResponse.userID
  //       this.router.navigateByUrl(`/auth/register/third-party/facebook?accessToken=${accessToken}&userId=${userId}`)
  //     } else {
  //       await Swal.fire({
  //         position: 'center',
  //         icon: 'error',
  //         title: 'Facebook login failed',
  //         text: 'Please try again',
  //         showConfirmButton: true
  //       })
  //     }
  //   })
  // }

  loginOrRegisterWithFacebook() {
    FB.login(async (fbResult: any) => {
      if (fbResult.authResponse) {
        // Gọi API để lấy thông tin người dùng, bao gồm email

        let email = '', firstName = '', lastName = '';
        FB.api('/me', { fields: 'name,email' }, (response: any) => {
          console.log(">>>>> first Res", response)
          email = response.email;
          const fullName = response.name.split(' ');
          firstName = fullName[0];
          lastName = fullName.length > 1 ? fullName[fullName.length - 1] : '';
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
  }}

  private async googleCallBack(res: CredentialResponse) {
    const decodedToken: any = jwtDecode(res.credential)
    console.log(decodedToken)

    const email = decodedToken.email
    const firstName = decodedToken.given_name
    const lastName = decodedToken.family_name
    const userId = decodedToken.sub
    const accessToken = res.credential

    const model = new LoginWithExternal(accessToken, userId, 'google');
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

  }

}
