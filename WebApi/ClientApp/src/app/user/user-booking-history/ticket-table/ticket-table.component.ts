import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core'
import { Ticket } from '../../../core/models/bookingHistory'
import { ConfirmCanceledTicketComponent } from '../../confirm-canceled-ticket/confirm-canceled-ticket.component'
import { BsModalService } from 'ngx-bootstrap/modal'

@Component({
  selector: 'app-ticket-table',
  templateUrl: './ticket-table.component.html',
  styleUrls: ['./ticket-table.component.scss']
})
export class TicketTableComponent implements OnInit {
  @Input() tickets: Ticket[] | undefined
  @Input() isUpcoming: boolean = false
  @Output() onConfirmCancel = new EventEmitter<void>()

  searchText: string = ''

  // Pagination
  currentPage: number = 1
  itemsPerPage: number = 10

  displayedItemsStart: number = 0
  displayedItemsEnd: number = 0

  constructor(
    private bsModalService: BsModalService) {}

  ngOnInit(): void {
    this.updateDisplayedItemsRange()
  }

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
      return this.tickets || []
    }

    const searchTextLower = this.searchText.toLowerCase()

    const filteredTickets = this.tickets.filter(ticket =>
      ticket.code.toLowerCase().includes(searchTextLower) ||
      ticket.passengerName.toLowerCase().includes(searchTextLower) ||
      ticket.trainName.toLowerCase().includes(searchTextLower)
    )

    this.displayedItemsStart = (this.currentPage - 1) * this.itemsPerPage + 1
    this.displayedItemsEnd = Math.min(this.currentPage * this.itemsPerPage, filteredTickets.length)

    return filteredTickets.slice(
      (this.currentPage - 1) * this.itemsPerPage,
      (this.currentPage - 1) * this.itemsPerPage + this.itemsPerPage
    )
  }

  pageChanged(event: any): void {
    this.currentPage = event.page
    this.updateDisplayedItemsRange()
  }

  private updateDisplayedItemsRange(): void {
    this.displayedItemsStart = Math.min(
      (this.currentPage - 1) * this.itemsPerPage + 1,
      this.filterTickets().length
    )
    this.displayedItemsEnd = Math.min(
      this.currentPage * this.itemsPerPage,
      this.filterTickets().length
    )
  }

  resetSearchText() {
    this.searchText = ''
    this.updateDisplayedItemsRange()
  }

}
