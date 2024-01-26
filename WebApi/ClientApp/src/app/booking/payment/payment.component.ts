import { Component, OnInit } from '@angular/core'
import { BookingService } from '../booking.service'
import { FormBuilder, FormGroup, Validators } from '@angular/forms'
import { PaymentService } from './payment.service'
import { PaymentInformation } from '../../core/models/paymentInformation'
import { PaymentPassenger, PaymentTicket } from '../../core/models/paymentTransaction'
import Swal from 'sweetalert2'
import { Router } from '@angular/router'

@Component({
  selector: 'app-payment',
  templateUrl: './payment.component.html',
  styleUrls: ['./payment.component.scss']
})
export class PaymentComponent implements OnInit {

  paymentInfo: PaymentInformation = {} as PaymentInformation
  paymentForm: FormGroup = new FormGroup({})
  ticketForm: FormGroup = new FormGroup({})
  totalAmount = 0

  constructor(public bookingService: BookingService,
              private paymentService: PaymentService,
              private router: Router,
              private fb: FormBuilder) {}

  ngOnInit(): void {
    setTimeout(() => {
      this.bookingService.currentStep = 4
    }, 0)

    this.loadTotalAmount()

    this.paymentForm = this.fb.group({
      nameOnCard: ['', Validators.required]
    })

    this.ticketForm = this.fb.group({
      passengers: [],
      tickets: [],
      trainId: [0],
      scheduleId: [0],
      paymentId: [0] // Lấy khi thanh toán thành công
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

  calculatedAge(dob: string) {
    const dobDate = new Date(dob)
    const timeDiff = Math.abs(Date.now() - dobDate.getTime())
    return Math.floor(timeDiff / (1000 * 3600 * 24) / 365.25)
  }

  onSubmit() {
    // if (this.paymentForm.invalid) {
    //   return
    // }

    let passengers: PaymentPassenger[] = []

    this.bookingService.currentSelectPassengers?.forEach((p) => {
      passengers.push({
        fullName: p.fullName,
        age: this.calculatedAge(p.dob),
        email: p.email,
        cardId: p.passportNumber,
        gender: p.title,
        phone: p.phoneNumber
      })
    })

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
    let tickets: PaymentTicket[] = []

    this.bookingService.currentSelectSeats?.forEach((s) => {
      tickets.push({
        carriageId: s.carriageId,
        seatId: s.id
      })
    })

    // const paymentTransaction: PaymentTransaction = {
    //   passengers: passengers,
    //   tickets: tickets,
    //   trainId: this.bookingService.currentSelectSchedule?.trainId || 0,
    //   scheduleId: this.bookingService.currentSelectSchedule?.id || 0,
    //   paymentId: 1 // Lấy khi thanh toán thành công
    // }

    this.ticketForm.patchValue({
      passengers: passengers,
      tickets: tickets,
      trainId: this.bookingService.currentSelectSchedule?.trainId || 0,
      scheduleId: this.bookingService.currentSelectSchedule?.id || 0,
      paymentId: 1 // Lấy khi thanh toán thành công
    })

    this.bookingService.addTicket(this.ticketForm.value).subscribe({
      next: (res) => {
        // Swal.fire({
        //   icon: 'success',
        //   title: 'Đặt vé thành công',
        //   showConfirmButton: false,
        //   timer: 1500
        // })
        this.router.navigateByUrl('/payment-success')
      },
      error: (err) => {
        console.log(err)
        Swal.fire({
          icon: 'error',
          title: 'Oops...',
          text: err.message
        })
      }
    })
  }
}
