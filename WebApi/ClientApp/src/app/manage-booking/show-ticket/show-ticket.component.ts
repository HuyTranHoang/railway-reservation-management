import { Component, OnInit } from '@angular/core'
import { ManageBookingService } from '../manage-booking.service'
import { Router } from '@angular/router'
import Swal from 'sweetalert2'

@Component({
  selector: 'app-show-ticket',
  templateUrl: './show-ticket.component.html',
  styleUrls: ['./show-ticket.component.scss']
})
export class ShowTicketComponent implements OnInit {

  status: string | undefined

  departureStationName: string | undefined
  arrivalStationName: string | undefined
  departureTime: Date | undefined
  arrivalTime: Date | undefined

  constructor(public manageService: ManageBookingService,
              private router: Router) { }

  ngOnInit() {
    if (!this.manageService.currentTicket) {
      this.router.navigate(['management'])
      Swal.fire({
        icon: 'error',
        title: 'Oops...',
        text: 'Please enter your email and code again!'
      })
    }

    this.loadSchedule()
  }

  loadSchedule() {
    if (!this.manageService.currentTicket) {
      return
    }

    this.manageService.getScheduleById(this.manageService.currentTicket?.scheduleId).subscribe({
      next: (response) => {
        this.arrivalStationName = response.arrivalStationName
        this.departureStationName = response.departureStationName
        this.arrivalTime = new Date(response.arrivalTime)
        this.departureTime = new Date(response.departureTime)

        if (this.manageService.currentTicket && this.manageService.currentTicket.isCancel) {
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
