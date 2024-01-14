import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {ScheduleAndTicketPricesComponent} from './schedule-and-ticket-prices.component';
import {ScheduleComponent} from './schedule/schedule.component';
import {DistanceFareComponent} from './distance-fare/distance-fare.component';
import {RoundTripComponent} from './round-trip/round-trip.component';
import {ScheduleAndTicketPricesRoutingModule} from './schedule-and-ticket-prices-routing.module';
import {SharedModule} from '../shared/shared.module';
import {RoundTripModule} from './round-trip/round-trip.module';
import {DistanceFareModule} from './distance-fare/distance-fare.module';

@NgModule({
  declarations: [
    ScheduleAndTicketPricesComponent,
    ScheduleComponent,
    RoundTripComponent,
    DistanceFareComponent,
  ],
  imports: [
    CommonModule,
    SharedModule,

    // Child Module
    RoundTripModule,
    DistanceFareModule,

    // Routing
    ScheduleAndTicketPricesRoutingModule,
  ],
})
export class ScheduleAndTicketPricesModule {
}
