import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/auth/auth.service';

@Component({
  selector: 'app-user-booking-history-header',
  templateUrl: './user-booking-history-header.component.html',
  styleUrls: ['./user-booking-history-header.component.scss']
})
export class UserBookingHistoryHeaderComponent implements OnInit {
  image = 'assets/booking.jpg'

  constructor(public authService : AuthService){

  }

  ngOnInit(): void {

  }
}
