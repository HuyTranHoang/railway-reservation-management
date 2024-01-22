import { Component, OnInit, ViewChild } from '@angular/core';
import { TabsetComponent } from 'ngx-bootstrap/tabs';
import { AuthService } from 'src/app/auth/auth.service';

@Component({
  selector: 'app-user-booking',
  templateUrl: './user-booking.component.html',
  styleUrls: ['./user-booking.component.scss']
})
export class UserBookingComponent implements OnInit {
  image = 'assets/booking.jpg'

  constructor(public authService : AuthService){

  }

  ngOnInit(): void {

  }
}
