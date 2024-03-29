import { Component, EventEmitter, Input, Output } from '@angular/core'
import { Schedule, ScheduleCarriageType } from '../../../core/models/schedule'
import { DepartureService } from '../departure.service'

@Component({
  selector: 'app-departure-select',
  templateUrl: './departure-select.component.html',
  styleUrls: ['./departure-select.component.scss']
})
export class DepartureSelectComponent {

  @Output() bookNow = new EventEmitter<Schedule>();
  openStates: boolean[] = []; // Array to track open/closed state for each item
  currentSelectSchedule: Schedule | undefined;
  currentDate = new Date();

  constructor(public departureService: DepartureService) {}

  onChevronClick(index: number) {
    this.openStates[index] = !this.openStates[index]; // Toggle the state for the specific item
  }

  onBookNowClick(item: Schedule) {
    if (!item.selectedCarriageType) {
      item.selectedCarriageType = item.scheduleCarriageTypes[0];
    }

    for (let i = 0; i < this.departureService.schedules.length; i++) {
      this.openStates[i] = false;
    }

    this.bookNow.emit(item); // Emit đến departure.component.ts
  }

  onSelectSeatTypeClick(item: Schedule, type: ScheduleCarriageType) {
    item.selectedCarriageType = type;
  }

  isExpired(item: Schedule) {
    const date = new Date(item.departureTime);
    return date < this.currentDate;
  }

}
