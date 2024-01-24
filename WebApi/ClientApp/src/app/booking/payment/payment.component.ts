import { Component, OnInit } from '@angular/core'
import { BookingService } from '../booking.service'
import { FormBuilder, FormGroup, Validators } from '@angular/forms'

@Component({
  selector: 'app-payment',
  templateUrl: './payment.component.html',
  styleUrls: ['./payment.component.scss']
})
export class PaymentComponent implements OnInit {

  paymentForm: FormGroup = new FormGroup({})
  totalAmount = 0

  constructor(public bookingService: BookingService, private fb: FormBuilder) {}

  ngOnInit(): void {
    setTimeout(() => {
      this.bookingService.currentStep = 4
    }, 0)

    this.loadTotalAmount()

    this.paymentForm = this.fb.group({
      nameOnCard: ['', Validators.required]
    })
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
