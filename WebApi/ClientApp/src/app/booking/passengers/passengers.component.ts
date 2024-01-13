import { Component, OnInit } from '@angular/core'
import { BookingService } from '../booking.service'

@Component({
  selector: 'app-passengers',
  templateUrl: './passengers.component.html',
  styleUrls: ['./passengers.component.scss']
})
export class PassengersComponent implements OnInit{

  constructor(private bookingService: BookingService) {}

  ngOnInit(): void {
    setTimeout(() => {
      this.bookingService.currentStep = 2;
    }, 0);
  }

}
