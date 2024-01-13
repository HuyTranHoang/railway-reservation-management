import { Component, OnInit } from '@angular/core'
import { Router } from '@angular/router'
import { BookingService } from './booking.service'

@Component({
  selector: 'app-booking',
  templateUrl: './booking.component.html',
  styleUrls: ['./booking.component.scss']
})
export class BookingComponent implements OnInit {

  constructor(public bookingService: BookingService) {}

  ngOnInit(): void {
  }

}
