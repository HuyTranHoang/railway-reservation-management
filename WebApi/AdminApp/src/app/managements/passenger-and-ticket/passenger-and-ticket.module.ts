import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {PassengerAndTicketComponent} from './passenger-and-ticket.component';
import {PassengerComponent} from './passenger/passenger.component';
import {TicketComponent} from './ticket/ticket.component';
import {PassengerAndTicketRoutingModule} from './passenger-and-ticket-routing.module';
import {SharedModule} from '../shared/shared.module';
import { AddPassengerComponent } from './passenger/add-passenger/add-passenger.component';
import {NbOptionModule, NbSelectModule} from '@nebular/theme';


@NgModule({
  declarations: [
    PassengerAndTicketComponent,
    PassengerComponent,
    TicketComponent,
    AddPassengerComponent,
  ],
  imports: [
    CommonModule,
    SharedModule,
    PassengerAndTicketRoutingModule,
    NbOptionModule,
    NbSelectModule,
  ],
})
export class PassengerAndTicketModule {
}
