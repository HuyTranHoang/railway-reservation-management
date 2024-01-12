import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {ScheduleAndTicketPricesComponent} from './schedule-and-ticket-prices.component';
import {ScheduleComponent} from './schedule/schedule.component';
import {DistanceFareComponent} from './distance-fare/distance-fare.component';
import {RoundTripComponent} from './round-trip/round-trip.component';
import {ScheduleAndTicketPricesRoutingModule} from './schedule-and-ticket-prices-routing.module';
import { AddRoundTripComponent } from './round-trip/add-round-trip/add-round-trip.component';
import { ConfirmDeleteRoundTripComponent } from './round-trip/confirm-delete-round-trip/confirm-delete-round-trip.component';
import { EditRoundTripComponent } from './round-trip/edit-round-trip/edit-round-trip.component';
import { ShowRoundTripComponent } from './round-trip/show-round-trip/show-round-trip.component';
import { SharedModule } from '../shared/shared.module';
import { NbOptionModule, NbSelectModule } from '@nebular/theme';




@NgModule({
  declarations: [
    ScheduleAndTicketPricesComponent,
    ScheduleComponent,
    DistanceFareComponent,
    RoundTripComponent,
    AddRoundTripComponent,
    ConfirmDeleteRoundTripComponent,
    EditRoundTripComponent,
    ShowRoundTripComponent,
  ],
  imports: [
    CommonModule,
    SharedModule,
    ScheduleAndTicketPricesRoutingModule,
    NbOptionModule,
    NbSelectModule,
  ],
})
export class ScheduleAndTicketPricesModule {
}
