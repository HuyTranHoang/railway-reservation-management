import { Component, OnInit } from '@angular/core'
import { BookingService } from '../booking.service'
import { FormBuilder, FormGroup } from '@angular/forms'
import { PaymentService } from './payment.service'
import { PaymentInformation } from '../../core/models/paymentInformation'
import { PaymentPassenger, PaymentTicket } from '../../core/models/paymentTransaction'
import Swal from 'sweetalert2'
import { Router } from '@angular/router'
import * as signalR from '@microsoft/signalr';
import { AuthService } from '../../auth/auth.service'
import { User } from '../../core/models/auth/user'

@Component({
  selector: 'app-payment',
  templateUrl: './payment.component.html',
  styleUrls: ['./payment.component.scss']
})
export class PaymentComponent implements OnInit {

  private hubConnection: signalR.HubConnection | undefined;

  paymentStatus = 'Pending'

  currentUser: User = {} as User
  paymentInfo: PaymentInformation = {} as PaymentInformation
  ticketForm: FormGroup = new FormGroup({})
  totalAmount = 0

  constructor(public bookingService: BookingService,
              private authService: AuthService,
              private paymentService: PaymentService,
              private router: Router,
              private fb: FormBuilder) {}

  ngOnInit(): void {
    setTimeout(() => {
      this.bookingService.currentStep = 4
    }, 0)

    this.authService.user$.subscribe({
      next: (user) => {
        if (user) this.currentUser = user
      }
    })

    this.loadTotalAmount()

    this.ticketForm = this.fb.group({
      passengers: [],
      tickets: [],
      trainId: [0],
      scheduleId: [0],
      paymentId: [0] // Lấy khi thanh toán thành công
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


    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:5001/paymentHub')
      .build();

    this.hubConnection.start().then(() => {
      console.log('SignalR Connected');
    });

    this.hubConnection.on('PaymentStatus', (message: string) => {
      console.log('PaymentStatus:', message);
      this.paymentService.setPaymentStatus(message);
    });

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

  createPaymentUrl() {
    this.paymentInfo.orderType = 'booking';
    this.paymentInfo.amount = this.totalAmount; // Sử dụng tổng số tiền cần thanh toán
    this.paymentInfo.orderDescription = 'Booking ticket';
    this.paymentInfo.name = this.currentUser.firstName + ' ' + this.currentUser.lastName;

    this.paymentService.createPaymentUrl(this.paymentInfo).subscribe({
      next: (res: any) => {
        console.log(res.paymentUrl);

        // Lưu trạng thái thanh toán khi đã tạo URL thành công
        // this.paymentStatus = 'PaymentPending';

        // Mở một cửa sổ mới và chuyển hướng đến URL thanh toán
        window.open(res.paymentUrl, '_blank');
      },
      error: (err: any) => {
        console.log(err);
      }
    });
  }


  addTicket() {
    this.paymentService.addPaymentByEmail(this.currentUser.email).subscribe({
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

        this.ticketForm.patchValue({
          passengers: passengers,
          tickets: tickets,
          trainId: this.bookingService.currentSelectSchedule?.trainId || 0,
          scheduleId: this.bookingService.currentSelectSchedule?.id || 0,
          paymentId: res.paymentId
        })

        this.bookingService.addTicket(this.ticketForm.value).subscribe({
          next: (res) => {
            this.hubConnection?.stop()
            this.router.navigateByUrl('/payment-success')
          },
          error: (err) => {
            console.log(err)
            Swal.fire({ icon: 'error', title: 'Oops...', text: 'Something went wrong!' })
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
