import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {ScheduleAndTicketPricesComponent} from './schedule-and-ticket-prices.component';
import {ScheduleComponent} from './schedule/schedule.component';
import {DistanceFareComponent} from './distance-fare/distance-fare.component';
import {RoundTripComponent} from './round-trip/round-trip.component';
import {ScheduleAndTicketPricesRoutingModule} from './schedule-and-ticket-prices-routing.module';
import { ShowDistanceFareComponent } from './distance-fare/show-distance-fare/show-distance-fare.component';
import { AddDistanceFareComponent } from './distance-fare/add-distance-fare/add-distance-fare.component';
import { EditDistanceFareComponent } from './distance-fare/edit-distance-fare/edit-distance-fare.component';
import { ConfirmDeleteDistanceFareComponent } from './distance-fare/confirm-delete-distance-fare/confirm-delete-distance-fare.component';
import { SharedModule } from '../shared/shared.module';
import {RoundTripModule} from './round-trip/round-trip.module';


@NgModule({
  declarations: [
    ScheduleAndTicketPricesComponent,
    ScheduleComponent,
    RoundTripComponent,
    DistanceFareComponent,
    AddDistanceFareComponent,
    EditDistanceFareComponent,
    ShowDistanceFareComponent,
    ConfirmDeleteDistanceFareComponent,

  ],
  imports: [
    CommonModule,
    SharedModule,

    // Child Module
    RoundTripModule,

    // Routing
    ScheduleAndTicketPricesRoutingModule,
  ],
})
export class ScheduleAndTicketPricesModule {
}
