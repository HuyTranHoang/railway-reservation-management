import { Component, OnInit } from '@angular/core'
import { ActivatedRoute } from '@angular/router'
import { ManageBookingService } from '../manage-booking.service'

@Component({
  selector: 'app-show-ticket',
  templateUrl: './show-ticket.component.html',
  styleUrls: ['./show-ticket.component.scss']
})
export class ShowTicketComponent implements OnInit {
  email: string = ''
  code: string = ''
  passengerName: string = ''
  trainName: string = ''
  carriageName: string = ''
  seatName: string = ''
  scheduleName: string = ''
  price: number = 0
  scheduleId: number = 0
  departureStationName: string | undefined
  arrivalStationName: string | undefined
  departureTime: string | undefined
  arrivalTime: string | undefined


  constructor(private route: ActivatedRoute, private manageService: ManageBookingService) { }

  ngOnInit() {
    this.route.queryParams.subscribe(params => {
      this.email = params['email']
      this.code = params['code']
      this.passengerName = params['passengerName']
      this.trainName = params['trainName']
      this.carriageName = params['carriageName']
      this.seatName = params['seatName']
      this.scheduleName = params['scheduleName']
      this.price = params['price']
      this.scheduleId = params['scheduleId']
    })

    this.loadSchedule()
  }

  loadSchedule() {
    this.manageService.getScheduleById(this.scheduleId).subscribe({
      next: (response) => {
        this.arrivalStationName = response.arrivalStationName
        this.departureStationName = response.departureStationName
        this.arrivalTime = response.arrivalTime
        this.departureTime = response.departureTime
      },
      error: (error: any) => {
        console.log(error)
      }
    })
  }

}
