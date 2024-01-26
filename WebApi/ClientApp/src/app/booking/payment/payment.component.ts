import { Component, OnInit } from '@angular/core'
import { BookingService } from '../booking.service'
import { FormBuilder, FormGroup, Validators } from '@angular/forms'
import { PaymentService } from './payment.service'
import { PaymentInformation } from '../../core/models/paymentInformation'

@Component({
  selector: 'app-payment',
  templateUrl: './payment.component.html',
  styleUrls: ['./payment.component.scss']
})
export class PaymentComponent implements OnInit {

  paymentInfo: PaymentInformation = {} as PaymentInformation
  paymentForm: FormGroup = new FormGroup({})
  totalAmount = 0

  constructor(public bookingService: BookingService,
              private paymentService: PaymentService,
              private fb: FormBuilder) {}

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

    this.paymentInfo.orderType = 'booking'
    this.paymentInfo.amount = 50000
    this.paymentInfo.orderDescription = 'Booking ticket'
    this.paymentInfo.name = 'Huy nek'

    this.paymentService.createPaymentUrl(this.paymentInfo).subscribe({
      next: (res: any) => {
        console.log(res.paymentUrl)
        // window.location.href = res.data
      },
      error: (err: any) => {
        console.log(err)
      }
    })
  }
}
