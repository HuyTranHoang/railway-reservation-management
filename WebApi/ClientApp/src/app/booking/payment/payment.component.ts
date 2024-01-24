import { Component, OnInit } from '@angular/core'
import { BookingService } from '../booking.service'

@Component({
  selector: 'app-payment',
  templateUrl: './payment.component.html',
  styleUrls: ['./payment.component.scss']
})
export class PaymentComponent implements OnInit {

  totalAmount = 0

  constructor(public bookingService: BookingService) {}

  ngOnInit(): void {
    setTimeout(() => {
      this.bookingService.currentStep = 4
    }, 0)

    this.loadTotalAmount()
  }

  loadTotalAmount() {

    const distancePrice = this.bookingService.currentSelectSchedule?.price || 0
    const carriageTypePrice = this.bookingService.currentSelectSchedule?.selectedCarriageType?.serviceCharge || 0

    if (this.bookingService.currentSelectSeats) {
      this.totalAmount = this.bookingService.currentSelectSeats
        .reduce((total, seat) =>
          total + seat.serviceCharge + distancePrice + carriageTypePrice, 0) || 0
    }
  }

  onSubmit() {

  }

}
