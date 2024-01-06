import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {PassengerAndTicketComponent} from './passenger-and-ticket.component';
import {PassengerComponent} from './passenger/passenger.component';
import {TicketComponent} from './ticket/ticket.component';
import {PassengerAndTicketRoutingModule} from './passenger-and-ticket-routing.module';


@NgModule({
  declarations: [
    PassengerAndTicketComponent,
    PassengerComponent,
    TicketComponent,
  ],
  imports: [
    CommonModule,
    PassengerAndTicketRoutingModule,
  ],
})
export class PassengerAndTicketModule {
}
