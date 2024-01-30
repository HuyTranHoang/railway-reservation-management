import { Component, EventEmitter, Input, Output } from '@angular/core'
import { Ticket } from '../../../core/models/bookingHistory'
import { ConfirmCanceledTicketComponent } from '../../confirm-canceled-ticket/confirm-canceled-ticket.component'
import { BsModalService } from 'ngx-bootstrap/modal'

@Component({
  selector: 'app-ticket-table',
  templateUrl: './ticket-table.component.html',
  styleUrls: ['./ticket-table.component.scss']
})
export class TicketTableComponent {
  @Input() tickets: Ticket[] | undefined
  @Input() isUpcoming: boolean = false
  @Output() onConfirmCancel = new EventEmitter<void>()

  searchText: string = ''

  constructor(
    private bsModalService: BsModalService) {}

  openConfirmModal(ticket: Ticket) {
    const modalRef = this.bsModalService.show(ConfirmCanceledTicketComponent, {
      initialState: { ticket }
    })

    modalRef.content?.onConfirmCancel.subscribe(() => {
      this.onConfirmCancel.emit()
    })
  }

  filterTickets(): Ticket[] {
    if (!this.tickets || !this.searchText) {
      return this.tickets || [];
    }

    const searchTextLower = this.searchText.toLowerCase();
    return this.tickets.filter(ticket =>
        ticket.code.toLowerCase().includes(searchTextLower) ||
        ticket.passengerName.toLowerCase().includes(searchTextLower) ||
        ticket.trainName.toLowerCase().includes(searchTextLower)
    );
  }

}
