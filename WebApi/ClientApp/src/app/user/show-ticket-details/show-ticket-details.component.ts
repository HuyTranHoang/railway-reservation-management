import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core'
import { Ticket } from '../../core/models/bookingHistory'
import { BsModalRef } from 'ngx-bootstrap/modal'

import { ManageBookingService } from '../../manage-booking/manage-booking.service'

@Component({
  selector: 'app-show-ticket-details',
  templateUrl: './show-ticket-details.component.html',
  styleUrls: ['./show-ticket-details.component.scss']
})
export class ShowTicketDetailsComponent implements OnInit, OnChanges {

  @Input() ticket: Ticket | undefined

  status: string | undefined

  departureStationName: string | undefined
  arrivalStationName: string | undefined
  departureTime: Date | undefined
  arrivalTime: Date | undefined

  constructor(private modalRef: BsModalRef,
              private manageService: ManageBookingService) { }

  ngOnChanges(changes: SimpleChanges): void {
    if ('ticket' in changes) {
      this.loadSchedule();
    }
  }

  ngOnInit(): void {
    this.loadSchedule()
  }

  dismiss() {
    this.modalRef.hide()
  }

  loadSchedule() {
    if (this.ticket) {
      this.manageService.getScheduleById(this.ticket.scheduleId).subscribe({
        next: (response) => {

          this.arrivalStationName = response.arrivalStationName
          this.departureStationName = response.departureStationName
          this.arrivalTime = new Date(response.arrivalTime)
          this.departureTime = new Date(response.departureTime)

          if (this.ticket && this.ticket.isCancel) {
            this.status = 'Cancelled'
          } else if (this.departureTime.getTime() < new Date().getTime()) {
            this.status = 'Departed'
          } else {
            this.status = 'Upcoming'
          }

        },
        error: (error: any) => {
          console.log(error)
        }
      })
    }
  }

}
