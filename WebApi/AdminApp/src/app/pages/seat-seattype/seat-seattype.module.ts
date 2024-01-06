import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SeatComponent } from './seat/seat.component';
import { SeattypeComponent } from './seattype/seattype.component';
import { SeatSeatTypeRoute, routedComponents } from './seat-seattype-routing.module';



@NgModule({
  declarations: [
      ...routedComponents
  ],
  imports: [
    SeatSeatTypeRoute
  ]
})
export class SeatSeattypeModule { }
