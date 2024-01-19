import { Component, OnInit } from '@angular/core'
import { BookingService } from '../booking.service'

@Component({
  selector: 'app-seat-selection',
  templateUrl: './seat-selection.component.html',
  styleUrls: ['./seat-selection.component.scss']
})
export class SeatSelectionComponent implements OnInit {

  constructor(private bookingService: BookingService) {}

  ngOnInit(): void {
    setTimeout(() => {
      this.bookingService.currentStep = 2;
    }, 0);
  }

}
