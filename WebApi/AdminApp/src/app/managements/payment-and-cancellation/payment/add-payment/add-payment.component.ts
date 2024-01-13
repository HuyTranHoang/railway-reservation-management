import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {NbGlobalPhysicalPosition, NbToastrService} from '@nebular/theme';
import {PaymentService} from '../payment.service';
import {UserService} from '../../../services/user.service';
import {ApplicationUser} from '../../../../@models/applicationUser';

@Component({
  selector: 'ngx-add-payment',
  templateUrl: './add-payment.component.html',
  styleUrls: ['./add-payment.component.scss'],
})
export class AddPaymentComponent implements OnInit {

  user: ApplicationUser | undefined;
  paymentForm: FormGroup = this.fb.group({});
  isSubmitted = false;
  errorMessages: string[] = [];

  constructor(private paymentService: PaymentService,
              private userService: UserService,
              private toastrService: NbToastrService,
              private fb: FormBuilder) {
  }

  ngOnInit(): void {
    this.initForm();
  }

  initForm() {
    this.paymentForm = this.fb.group({
      aspNetUserId: ['', Validators.required],
      email: ['', Validators.required],
      status: [''],
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
          this.paymentService.addPayment(this.paymentForm.value).subscribe({
            next: (paymentRes) => {
              this.showToast('success', 'Success', 'Add payment successfully!');
              this.paymentForm.reset();
              this.isSubmitted = false;
              this.errorMessages = [];
            },
            error: (err) => {
              this.showToast('danger', 'Failed', 'Add carriage type failed!');
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
