import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BookingRoutingModule } from './booking-routing.module';
import { BookingComponent } from './booking.component';
import { DepartureComponent } from './departure/departure.component'
import { SharedModule } from '../shared/shared.module';
import { DepartureInfoComponent } from './departure/departure-info/departure-info.component';
import { DepartureInputFieldComponent } from './departure/departure-input-field/departure-input-field.component';
import { DepartureDatepickerFieldComponent } from './departure/departure-datepicker-field/departure-datepicker-field.component';
import { DepartureSelectComponent } from './departure/departure-select/departure-select.component';
import { PassengersComponent } from './passengers/passengers.component';
import { SeatSelectionComponent } from './seat-selection/seat-selection.component'
import { RouterModule } from '@angular/router'

@NgModule({
  declarations: [
    BookingComponent,
    DepartureComponent,
    DepartureInfoComponent,
    DepartureInputFieldComponent,
    DepartureDatepickerFieldComponent,
    DepartureSelectComponent,
    PassengersComponent,
    SeatSelectionComponent
  ],
  imports: [
    CommonModule,
    BookingRoutingModule,
    SharedModule
  ],
  exports :[
    DepartureInputFieldComponent,
    DepartureDatepickerFieldComponent,
    DepartureComponent,
    DepartureInfoComponent,
  ]
})
export class BookingModule { }
