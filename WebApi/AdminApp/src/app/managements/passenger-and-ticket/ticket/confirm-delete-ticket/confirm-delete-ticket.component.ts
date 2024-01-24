import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {NbDialogRef, NbGlobalPhysicalPosition, NbToastrService} from '@nebular/theme';
import {CancellationService} from '../../../payment-and-cancellation/cancellation/cancellation.service';
import {Cancellation} from '../../../../@models/cancellation';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';

@Component({
  selector: 'ngx-confirm-delete-ticket',
  templateUrl: './confirm-delete-ticket.component.html',
  styleUrls: ['./confirm-delete-ticket.component.scss'],
})
export class ConfirmDeleteTicketComponent implements OnInit {
  @Input() id: number;
  @Input() code: string;
  @Output() onConfirmCancel = new EventEmitter<void>();

  cancellationForm: FormGroup = this.fb.group({});

  constructor(protected ref: NbDialogRef<ConfirmDeleteTicketComponent>,
              private cancellationService: CancellationService,
              private fb: FormBuilder,
              private toastrService: NbToastrService) {
  }

  ngOnInit(): void {
    this.initForm();
  }

  dismiss() {
    this.ref.close();
  }

  initForm() {
    this.cancellationForm = this.fb.group({
      ticketId: ['', [Validators.required]],
      reason: [''],
      status: [''],
    });
  }

  onCancel() {
    this.cancellationForm.patchValue({
      ticketId: this.id,
      reason: 'Staff canceled',
      status: 'Canceled',
    });

    this.cancellationService.addCancellation(this.cancellationForm.value).subscribe({
      next: () => {
        this.showToast('success', 'Success', 'Ticket canceled successfully');
        this.onConfirmCancel.emit();
        this.dismiss();
      },
      error: (err) => {
        if (err.error.errors) {
          err.error.errors.forEach((element: string) => {
            this.showToast('danger', 'Error', element);
          });
        } else {
          this.showToast('danger', 'Error', 'Failed to cancel ticket');
        }
        this.dismiss();
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
