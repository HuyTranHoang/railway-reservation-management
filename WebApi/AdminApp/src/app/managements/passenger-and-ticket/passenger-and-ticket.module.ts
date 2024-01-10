import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {PassengerAndTicketComponent} from './passenger-and-ticket.component';
import {PassengerComponent} from './passenger/passenger.component';
import {TicketComponent} from './ticket/ticket.component';
import {PassengerAndTicketRoutingModule} from './passenger-and-ticket-routing.module';
import {SharedModule} from '../shared/shared.module';


@NgModule({
  declarations: [
    PassengerAndTicketComponent,
    PassengerComponent,
    TicketComponent,
  ],
  imports: [
    CommonModule,
    SharedModule,
    PassengerAndTicketRoutingModule,
  ],
})
export class PassengerAndTicketModule {
}
