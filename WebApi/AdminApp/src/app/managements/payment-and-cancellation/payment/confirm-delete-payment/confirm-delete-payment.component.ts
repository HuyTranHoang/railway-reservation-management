import {Component, EventEmitter, Input, Output} from '@angular/core';
import {NbDialogRef, NbGlobalPhysicalPosition, NbToastrService} from '@nebular/theme';
import {PaymentService} from '../payment.service';

@Component({
  selector: 'ngx-confirm-delete-payment',
  templateUrl: './confirm-delete-payment.component.html',
  styleUrls: ['./confirm-delete-payment.component.scss'],
})
export class ConfirmDeletePaymentComponent {

  @Input() id: number;
  @Input() name: string;
  @Input() transactionId: string;
  @Output() onConfirmDelete = new EventEmitter<void>();

  constructor(protected ref: NbDialogRef<ConfirmDeletePaymentComponent>,
              private paymentService: PaymentService,
              private toastrService: NbToastrService) {
  }

  dismiss() {
    this.ref.close();
  }

  onDelete() {
    this.paymentService.deletePayment(this.id).subscribe({
      next: _ => {
        this.showToast('success', 'Success', 'Delete payment successfully!');
        this.onConfirmDelete.emit();
        this.ref.close();
      },
      error: _ => {
        this.showToast('danger', 'Failed', 'Delete payment failed!');
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
