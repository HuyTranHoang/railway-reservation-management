import { UserService } from '../user.service'
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core'
import { FormBuilder, FormGroup, Validators } from '@angular/forms'
import { BsModalRef } from 'ngx-bootstrap/modal'
import Swal from 'sweetalert2'
import { Ticket } from '../../core/models/bookingHistory'

@Component({
  selector: 'app-confirm-canceled-ticket',
  templateUrl: './confirm-canceled-ticket.component.html',
  styleUrls: ['./confirm-canceled-ticket.component.scss']
})
export class ConfirmCanceledTicketComponent implements OnInit {

  @Input() ticket: Ticket | undefined
  @Output() onConfirmCancel = new EventEmitter<void>()

  cancellationForm: FormGroup = this.fb.group({})

  constructor(private modalRef: BsModalRef,
              private fb: FormBuilder,
              private userService: UserService) { }

  ngOnInit(): void {
    this.initForm()
  }

  dismiss() {
    this.modalRef.hide()
  }

  initForm() {
    this.cancellationForm = this.fb.group({
      ticketId: ['', [Validators.required]],
      reason: [''],
      status: ['Canceled']
    })
  }

  onCancel() {
    this.cancellationForm.patchValue({
      ticketId: this.ticket?.id
    })

    this.userService.canceledTicket(this.cancellationForm.value).subscribe({
      next: () => {
        this.onConfirmCancel.emit()
        this.dismiss()
      },
      error: (err) => {
        console.log(err)
        Swal.fire({
          title: 'Error',
          text: 'Something went wrong. Please try again.',
          icon: 'error',
          confirmButtonText: 'Ok'
        })
      }
    })
  }


}
