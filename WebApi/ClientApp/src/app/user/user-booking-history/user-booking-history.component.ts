import { Component, OnInit, ViewChild } from '@angular/core';
import { TabsetComponent } from 'ngx-bootstrap/tabs';
import { AuthService } from 'src/app/auth/auth.service';

@Component({
  selector: 'app-user-booking-history',
  templateUrl: './user-booking-history.component.html',
  styleUrls: ['./user-booking-history.component.scss']
})
export class UserBookingHistoryComponent implements OnInit {

  constructor(){
  }

  ngOnInit(): void {
  }
}
