import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {SeatComponent} from './seat/seat.component';
import {SeatTypeComponent} from './seat-type/seat-type.component';
import {SeatAndSeatTypeComponent} from './seat-and-seat-type.component';
import {SeatAndSeatTypeRoutingModule} from './seat-and-seat-type-routing.module';


@NgModule({
  declarations: [
    SeatComponent,
    SeatTypeComponent,
    SeatAndSeatTypeComponent,
  ],
  imports: [
    CommonModule,
    SeatAndSeatTypeRoutingModule,
  ],
})
export class SeatAndSeatTypeModule {
}
