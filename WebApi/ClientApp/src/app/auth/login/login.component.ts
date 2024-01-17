import { Component, OnInit } from '@angular/core'
import { FormBuilder, FormGroup, Validators } from '@angular/forms'
import { AuthService } from '../auth.service'
import { ActivatedRoute, Router } from '@angular/router'
import Swal from 'sweetalert2'
import { take } from 'rxjs'
import { LoginWithExternal } from '../../core/models/auth/loginWithExternal'

declare const FB: any

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['../login-and-register.component.scss']
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup = new FormGroup({})
  submitted = false
  errorMessages: string[] = []
  returnUrl: string = ''
  isResendEmail = false

  constructor(private authService: AuthService,
              private fb: FormBuilder,
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
    this.initializeForm()
  }

  initializeForm(): void {
    this.loginForm = this.fb.group({
      userName: ['', Validators.required],
      password: ['', Validators.required]
    });
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
        if (err.statusCode === 401 && err.message != "Invalid username or password") {
          this.isResendEmail = true
        }
      }
    })
  }

  resendEmailConfirmationLink() {
    this.router.navigateByUrl('/auth/send-email/resend-email-confirmation-link');
  }

  // loginWithFacebook() {
  //   FB.login(async (fbResult: any) => {
  //     if (fbResult.authResponse) {
  //       console.log(fbResult)
  //       const accessToken = fbResult.authResponse.accessToken
  //       const userId = fbResult.authResponse.userID
  //
  //       const model = new LoginWithExternal(accessToken, userId, 'facebook')
  //
  //       this.authService.loginWithThirdParty(model).subscribe({
  //         next: _ => {
  //           this.router.navigateByUrl('/')
  //         },
  //         error: err => {
  //           console.log(err.errors)
  //           this.errorMessages = err.errors
  //         }
  //       })
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
              // Người dùng chưa đăng ký, chuyển hướng đến trang đăng ký với thông tin Facebook
              this.router.navigateByUrl(`/auth/register/third-party/facebook?accessToken=${accessToken}&userId=${userId}`);
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
    });
  }

}
