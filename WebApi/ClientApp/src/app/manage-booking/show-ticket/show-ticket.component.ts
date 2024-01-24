import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ManageBookingService } from '../manage-booking.service';

@Component({
  selector: 'app-show-ticket',
  templateUrl: './show-ticket.component.html',
  styleUrls: ['./show-ticket.component.scss']
})
export class ShowTicketComponent {
  email: string = '';
  code: string = '';
  passengerName: any;
  trainName: any;
  carriageName: any;
  seatName: any;
  scheduleName: any;
  price: any;
  scheduleId: any;
  departureStationName : string | undefined;
  arrivalStationName : string | undefined;
  departureTime : string | undefined;
  arrivalTime : string| undefined;



  constructor(private route: ActivatedRoute , private manageService : ManageBookingService) { }

  ngOnInit() {
    this.route.queryParams.subscribe(params => {
      this.email = params['email'];
      this.code = params['code'];
      this.passengerName = params['passengerName'],
      this.trainName =  params['trainName'],
      this.carriageName =  params['carriageName'],
      this.seatName =  params['seatName'],
      this.scheduleName = params['scheduleName'],
      this.price = params['price'],
      this.scheduleId = params['scheduleId']
    });
    this.getCheduleById();
  }
  getCheduleById() {
    this.manageService.getScheduleById(this.scheduleId).subscribe({
      next: (response) => {
        this.arrivalStationName = response.arrivalStationName;
        this.departureStationName = response.departureStationName;
        this.arrivalTime = response.arrivalTime;
        this.departureTime = response.departureTime;
      },
      error: (error: any) => {
        console.log(error);
      }
    });
  }
  
}
