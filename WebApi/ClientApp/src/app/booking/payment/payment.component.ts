import { Component, OnInit } from '@angular/core'
import { BookingService } from '../booking.service'

@Component({
  selector: 'app-payment',
  templateUrl: './payment.component.html',
  styleUrls: ['./payment.component.scss']
})
export class PaymentComponent implements OnInit {

  constructor(public bookingService: BookingService) {}
  ngOnInit(): void {
    setTimeout(() => {
      this.bookingService.currentStep = 4
    }, 0)
  }

}
