import { Component, OnInit } from '@angular/core'
import { AuthService } from '../auth.service'
import { ActivatedRoute, Router } from '@angular/router'
import { take } from 'rxjs'
import { ConfirmEmail } from '../../core/models/auth/confirmEmail'
import Swal from 'sweetalert2'

@Component({
  selector: 'app-confirm-email',
  templateUrl: './confirm-email.component.html',
  styleUrls: ['./confirm-email.component.scss']
})
export class ConfirmEmailComponent implements OnInit {
  success = true

  constructor(private authService: AuthService,
              private router: Router,
              private activatedRoute: ActivatedRoute) {}

  ngOnInit(): void {
    this.authService.user$.pipe(take(1)).subscribe({
      next: (user) => {
        if (user) {
          this.router.navigateByUrl('/');
        } else {
          this.activatedRoute.queryParamMap.subscribe({
            next: (params: any) => {
              const token = params.get('token');
              const email = params.get('email');
              if (token && email) {
                this.confirmEmail(token, email);
              }
            }
          });
        }
      }
    });
  }


  private confirmEmail(token: string, email: string): void {
    const confirmEmail: ConfirmEmail = {
      token: token,
      email: email
    }

    this.authService.confirmEmail(confirmEmail).subscribe({
      next: (res: any) => {
        Swal.fire({
          title: 'Email Confirmed!',
          text: res.value.message,
          icon: 'success',
          confirmButtonText: 'Ok'
        })

        this.router.navigateByUrl('/auth/login');
      },
      error: (err: any) => {
        this.success = false;
        // interceptors handle error 400
      }
    });
  }


  resendEmailConfirmationLink() {
    this.router.navigateByUrl('/auth/send-email/resend-email-confirmation-link');
  }

}
