import { Component, OnInit } from '@angular/core'
import { BookingService } from '../booking.service'

@Component({
  selector: 'app-departure',
  templateUrl: './departure.component.html',
  styleUrls: ['./departure.component.scss', './departure.shared.scss']
})
export class DepartureComponent implements OnInit{


  isModify = false;

  constructor(private bookingService: BookingService) {}

  ngOnInit(): void {
    setTimeout(() => {
      this.bookingService.currentStep = 1;
    }, 0);
  }

  onModifyButtonClicked(isModified: boolean) {
    this.isModify = isModified;
  }

  onBookNowClick(id: number) {
    console.log('Book now clicked for train id: ', id);
  }

}
