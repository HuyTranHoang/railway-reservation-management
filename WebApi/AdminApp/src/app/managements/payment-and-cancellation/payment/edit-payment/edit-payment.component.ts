import {Component, OnInit} from '@angular/core';
import {ApplicationUser} from '../../../../@models/applicationUser';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {PaymentService} from '../payment.service';
import {UserService} from '../../../services/user.service';
import {NbGlobalPhysicalPosition, NbToastrService} from '@nebular/theme';
import {ActivatedRoute, Router} from '@angular/router';

@Component({
  selector: 'ngx-edit-payment',
  templateUrl: './edit-payment.component.html',
  styleUrls: ['./edit-payment.component.scss'],
})
export class EditPaymentComponent implements OnInit {


  user: ApplicationUser | undefined;
  paymentForm: FormGroup = this.fb.group({});
  isSubmitted = false;
  errorMessages: string[] = [];

  isLoading = false;

  constructor(private paymentService: PaymentService,
              private userService: UserService,
              private toastrService: NbToastrService,
              private activatedRoute: ActivatedRoute,
              private router: Router,
              private fb: FormBuilder) {
  }

  ngOnInit(): void {
    this.initForm();
  }

  initForm() {
    this.paymentForm = this.fb.group({
      id: ['', Validators.required],
      aspNetUserId: ['', Validators.required],
      email: ['', Validators.required],
      status: [''],
    });

    const id = this.activatedRoute.snapshot.params.id;
    this.isLoading = true;
    this.paymentService.getPaymentById(id).subscribe({
      next: (res) => {
        this.paymentForm.patchValue(res);
        this.isLoading = false;
      },
      error: (err) => {
        this.showToast('danger', 'Failed', 'Payment doest not exist!');
        this.router.navigateByUrl('/managements/payment-and-cancellation/payment');
      },
    });
  }

  onSubmit() {
    this.isSubmitted = true;
    this.errorMessages = [];

    this.userService.getUserByEmail(this.paymentForm.value.email).subscribe({
      next: (res) => {
        this.paymentForm.patchValue({
          aspNetUserId: res.id,
        });

        if (this.paymentForm.valid) {
          this.paymentService.updatePayment(this.paymentForm.value).subscribe({
            next: (paymentRes) => {
              this.showToast('success', 'Success', 'Update payment successfully!');
              this.isSubmitted = false;
              this.errorMessages = [];
            },
            error: (err) => {
              this.showToast('danger', 'Failed', 'Update payment failed!');
              this.errorMessages = err.error.errorMessages;
            },
          });
        }
      },
      error: (err) => {
        this.errorMessages = err.error.errors;
      },
    });

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
