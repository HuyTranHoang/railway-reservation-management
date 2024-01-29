import { UserService } from './../user.service';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BsModalRef } from 'ngx-bootstrap/modal';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-confirm-canceled-ticket',
  templateUrl: './confirm-canceled-ticket.component.html',
  styleUrls: ['./confirm-canceled-ticket.component.scss']
})
export class ConfirmCanceledTicketComponent implements OnInit{

    @Input() id: string | undefined;
    @Input() code: string | undefined;
    @Output() onConfirmCancel = new EventEmitter<void>();

    cancellationForm: FormGroup = this.fb.group({});

    constructor(private modalRef : BsModalRef,
                private fb : FormBuilder,
                private userService : UserService) { }

    ngOnInit(): void {
      this.initForm();
    }

    dismiss() {
      this.modalRef.hide();
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

      this.userService.canceledTicket(this.cancellationForm.value).subscribe({
        next: () => {
          Swal.fire({
            title: 'Ticket Canceled',
            text: 'Your ticket has been canceled successfully.',
            icon: 'success',
            confirmButtonText: 'Ok'
          })
          this.onConfirmCancel.emit();
          this.dismiss();
        },
        error: (err) => {
          if (err.error.errors) {
            this.dismiss();
          }
        },
      });
    }


}
