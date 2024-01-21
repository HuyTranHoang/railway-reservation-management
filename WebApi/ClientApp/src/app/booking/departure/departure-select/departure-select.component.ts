import { Component, EventEmitter, Input, Output } from '@angular/core'
import { Schedule } from '../../../core/models/schedule'
import { DepartureService } from '../departure.service'

@Component({
  selector: 'app-departure-select',
  templateUrl: './departure-select.component.html',
  styleUrls: ['./departure-select.component.scss']
})
export class DepartureSelectComponent {

  @Output() bookNow = new EventEmitter<number>();
  openStates: boolean[] = []; // Array to track open/closed state for each item

  constructor(public departureService: DepartureService) {}

  onChevronClick(index: number) {
    this.openStates[index] = !this.openStates[index]; // Toggle the state for the specific item
  }

  onBookNowClick(id: number) {
    this.bookNow.emit(id);
  }

}
