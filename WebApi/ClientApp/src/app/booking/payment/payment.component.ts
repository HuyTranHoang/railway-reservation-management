import { Component, OnDestroy, OnInit } from '@angular/core'
import { BookingService } from '../booking.service'
import { FormBuilder, FormGroup } from '@angular/forms'
import { PaymentService } from './payment.service'
import { PaymentInformation } from '../../core/models/paymentInformation'
import { PaymentPassenger, PaymentTicket } from '../../core/models/paymentTransaction'
import Swal from 'sweetalert2'
import { Router } from '@angular/router'
import * as signalR from '@microsoft/signalr'
import { AuthService } from '../../auth/auth.service'
import { User } from '../../core/models/auth/user'

@Component({
  selector: 'app-payment',
  templateUrl: './payment.component.html',
  styleUrls: ['./payment.component.scss']
})
export class PaymentComponent implements OnInit, OnDestroy {

  private hubConnection: signalR.HubConnection | undefined

  paymentStatus = 'Pending'

  currentUser: User = {} as User
  paymentInfo: PaymentInformation = {} as PaymentInformation
  ticketForm: FormGroup = new FormGroup({})
  totalAmount = 0

  transactionId = ''

  departureSubTotal = 0
  returnSubTotal = 0
  totalSeats = 0
  totalRoundTripDiscount = 0
  discountRoundTrip = 0

  constructor(public bookingService: BookingService,
              private authService: AuthService,
              private paymentService: PaymentService,
              private router: Router,
              private fb: FormBuilder) {}

  ngOnDestroy(): void {
    this.hubConnection?.stop()
  }

  ngOnInit(): void {
    setTimeout(() => {
      this.bookingService.currentStep = 4
    }, 0)

    this.authService.user$.subscribe({
      next: (user) => {
        if (user) this.currentUser = user
      }
    })


    this.totalSeats = this.bookingService.currentSelectSeats?.length || 0

    this.loadTotalAmount()

    this.ticketForm = this.fb.group({
      passengers: [],
      tickets: [],
      trainId: [],
      scheduleId: [],
      paymentId: [0], // Lấy khi thanh toán thành công,
      isRoundTrip: [this.bookingService.isRoundTrip]
    })

    this.paymentService.paymentStatus$.subscribe(status => {
      // Nếu trạng thái là Success, tự động thực hiện addTicket
      if (status === 'PaymentSuccess') {
        this.addTicket()
        this.paymentService.setPaymentStatus('Pending')
      } else if (status === 'PaymentFailed') {
        this.paymentStatus = 'PaymentFailed'
      } else if (status === 'PaymentCancel') {
        this.paymentStatus = 'PaymentCancel'
      }
    })

    if (!this.bookingService.currentBookingScheduleParams) {
      Swal.fire('Oops', 'Please select a valid departure station, arrival station and departure date', 'error')
      this.router.navigate(['/'])
    }

    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:5001/paymentHub', {
        accessTokenFactory: () => this.currentUser.jwt
      })
      .build()

    this.hubConnection.start().then(() => {
      console.log('SignalR Connected')
    })

    this.hubConnection.on('TransactionId', (message: string) => {
      console.log('TransactionId:', message)
      this.transactionId = message
    })

    this.hubConnection.on('PaymentStatus', (message: string) => {
      console.log('PaymentStatus:', message)
      this.paymentService.setPaymentStatus(message)
    })
  }


  loadTotalAmount() {
    let distancePrice = this.bookingService.currentSelectDepartureSchedule?.price || 0
    let carriageTypePrice = this.bookingService.currentSelectDepartureSchedule?.selectedCarriageType?.serviceCharge || 0

    if (this.bookingService.currentSelectSeats) {
      if (this.bookingService.isRoundTrip) {
        for (let i = 0; i < this.totalSeats / 2; i++) {
          this.totalAmount += this.bookingService.currentSelectSeats[i].serviceCharge
            + distancePrice + carriageTypePrice
        }
      } else {
        for (let i = 0; i < this.totalSeats; i++) {
          this.totalAmount += this.bookingService.currentSelectSeats[i].serviceCharge
            + distancePrice + carriageTypePrice
        }
      }

      this.departureSubTotal = this.totalAmount
    }

    if (this.bookingService.isRoundTrip) {
      distancePrice = this.bookingService.currentSelectReturnSchedule?.price || 0
      carriageTypePrice = this.bookingService.currentSelectReturnSchedule?.selectedCarriageType?.serviceCharge || 0

      if (this.bookingService.currentSelectSeats) {
        for (let i = this.totalSeats / 2; i < this.totalSeats; i++) {
          this.totalAmount += this.bookingService.currentSelectSeats[i].serviceCharge
            + distancePrice + carriageTypePrice
        }
      }

      this.paymentService.getRoundTripByTrainCompanyId(this.bookingService.currentSelectDepartureSchedule?.trainCompanyId || 0)
        .subscribe({
          next: (res) => {
            this.discountRoundTrip = res.discount
            this.returnSubTotal = this.totalAmount - this.departureSubTotal
            this.totalRoundTripDiscount = (this.returnSubTotal * this.discountRoundTrip) / 100
            this.returnSubTotal = this.returnSubTotal - this.totalRoundTripDiscount
            this.totalAmount = this.totalAmount - this.totalRoundTripDiscount
          },
          error: (err) => {
            console.log(err)
          }
        })
    }

    // if (this.bookingService.currentSelectSeats) {
    //   this.totalAmount = this.bookingService.currentSelectSeats
    //     .reduce((total, seat) =>
    //       total + seat.serviceCharge + distancePrice + carriageTypePrice, 0) || 0
    // }
  }

  calculatedAge(dob: string) {
    const dobDate = new Date(dob)
    const timeDiff = Math.abs(Date.now() - dobDate.getTime())
    return Math.floor(timeDiff / (1000 * 3600 * 24) / 365.25)
  }

  createPaymentUrl() {
    this.paymentInfo.orderType = 'booking'
    this.paymentInfo.amount = this.totalAmount // Sử dụng tổng số tiền cần thanh toán
    this.paymentInfo.orderDescription = 'Booking ticket'
    this.paymentInfo.name = this.currentUser.firstName + ' ' + this.currentUser.lastName

    this.paymentService.createPaymentUrl(this.paymentInfo).subscribe({
      next: (res: any) => {
        console.log(res.paymentUrl)

        // Lưu trạng thái thanh toán khi đã tạo URL thành công
        // this.paymentStatus = 'PaymentPending';

        // Mở một cửa sổ mới và chuyển hướng đến URL thanh toán
        window.open(res.paymentUrl, '_blank')
      },
      error: (err: any) => {
        console.log(err)
      }
    })
  }


  addTicket() {
    this.paymentService.addPaymentByEmail(this.currentUser.email, this.transactionId).subscribe({
      next: (res: any) => {
        console.log(res)

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

        let tickets: PaymentTicket[] = []

        this.bookingService.currentSelectSeats?.forEach((s) => {
          tickets.push({
            carriageId: s.carriageId,
            seatId: s.id
          })
        })

        let trainId: number[] = []
        let scheduleId: number[] = []

        if (this.bookingService.isRoundTrip) {
          trainId.push(this.bookingService.currentSelectDepartureSchedule?.trainId || 0)
          trainId.push(this.bookingService.currentSelectReturnSchedule?.trainId || 0)

          scheduleId.push(this.bookingService.currentSelectDepartureSchedule?.id || 0)
          scheduleId.push(this.bookingService.currentSelectReturnSchedule?.id || 0)
        } else {
          trainId.push(this.bookingService.currentSelectDepartureSchedule?.trainId || 0)
          scheduleId.push(this.bookingService.currentSelectDepartureSchedule?.id || 0)
        }


        this.ticketForm.patchValue({
          passengers: passengers,
          tickets: tickets,
          trainId: trainId,
          scheduleId: scheduleId,
          paymentId: res.paymentId,
          isRoundTrip: this.bookingService.isRoundTrip
        })

        this.bookingService.addTicket(this.ticketForm.value).subscribe({
          next: (res) => {
            this.hubConnection?.stop()
            this.router.navigateByUrl('/payment-success')
          },
          error: (err) => {
            console.log(err)
            Swal.fire({
              icon: 'error',
              title: 'Oops...',
              text: 'It seems someone has already booked the selected seat. Please choose another seat.'
            })

            this.router.navigateByUrl('/booking/seat-selection')
            this.hubConnection?.stop()
          }
        })
      },
      error: (err) => {
        console.log(err)
      }
    })

  }

  onSubmit() {
    this.createPaymentUrl()
  }

}
