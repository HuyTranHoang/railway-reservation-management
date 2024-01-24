import {Component, OnInit} from '@angular/core';
import {FormGroup, FormBuilder, Validators} from '@angular/forms';
import {Router, ActivatedRoute} from '@angular/router';
import {take} from 'rxjs/operators';
import {AuthService} from '../auth.service';
import {NbToastrService, NbGlobalPhysicalPosition} from '@nebular/theme';

@Component({
  selector: 'ngx-send-email',
  templateUrl: './send-email.component.html',
  styleUrls: ['./send-email.component.scss'],
})
export class SendEmailComponent implements OnInit {
  emailForm: FormGroup = new FormGroup({});
  submitted = false;
  mode: string = '';
  errorMessages: string[] = [];

  constructor(private authService: AuthService,
              private router: Router,
              private activatedRoute: ActivatedRoute,
              private fb: FormBuilder,
              private toastrService: NbToastrService) {
  }

  ngOnInit(): void {
    this.authService.user$.pipe(take(1)).subscribe({
      next: (user) => {
        if (user) {
          this.router.navigateByUrl('/');
        } else {
          const mode = this.activatedRoute.snapshot.paramMap.get('mode');
          if (mode) {
            this.mode = mode;
            this.initializeForm();
          }
        }
      },

    });
  }

  initializeForm() {
    this.emailForm = this.fb.group({
      email: ['', [Validators.required, Validators.pattern('^\\w+@[a-zA-Z_]+?\\.[a-zA-Z]{2,3}$')]],
    });
  }

  onSubmit() {
    this.submitted = true;
    this.errorMessages = [];

    if (this.emailForm.invalid) {
      return;
    }

    if (!this.mode) {
      return;
    }

    if (this.mode.includes('forgot-password')) {
      this.authService.forgotPasswordAdmin(this.emailForm.get('email')?.value).subscribe({
        next: (res: any) => {
          this.showToast('success', 'Success', 'Email sent successfully');
          this.router.navigateByUrl('/auth/login');
        },
        error: (err: any) => {
          this.errorMessages = [err.error.message];
          this.showToast('danger', 'Failed', 'Email sending failed!');
        },
      });
    }
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
