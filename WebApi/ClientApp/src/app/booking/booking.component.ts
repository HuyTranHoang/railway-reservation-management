import { Component, OnInit } from '@angular/core'
import { Router } from '@angular/router'
import { BookingService } from './booking.service'
import { Location } from '@angular/common'

@Component({
  selector: 'app-booking',
  templateUrl: './booking.component.html',
  styleUrls: ['./booking.component.scss']
})
export class BookingComponent implements OnInit {

  constructor(public bookingService: BookingService,
              private router: Router,
              private location: Location) {}

  ngOnInit(): void {
  }

  navigateBack(stepToNavigate: number): void {
    if (this.bookingService.currentStep >= stepToNavigate) {
      this.bookingService.currentStep = stepToNavigate;
      switch (stepToNavigate) {
        case 1:
          this.router.navigate(['/booking']);
          break;
        case 2:
          this.router.navigate(['/booking/seat-selection']);
          break;
        case 3:
          this.router.navigate(['/booking/passengers']);
          break;
        default:
          this.router.navigate(['/booking']);
      }
    }
  }

}
