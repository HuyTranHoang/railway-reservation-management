import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BookingRoutingModule } from './booking-routing.module';
import { BookingComponent } from './booking.component';
import { DepartureComponent } from './departure/departure.component'
import { SharedModule } from '../shared/shared.module'



@NgModule({
  declarations: [
    BookingComponent,
    DepartureComponent
  ],
  imports: [
    CommonModule,
    BookingRoutingModule,
    SharedModule
  ]
})
export class BookingModule { }
