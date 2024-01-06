import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { PassengerTicketComponent } from './passenger-ticket.component';
import { PassengerComponent } from './passenger/passenger.component';
import { TicketComponent } from './ticket/ticket.component';


const routes: Routes = [{
  path: '',
  component: PassengerTicketComponent,
  children: [
    {
      path: 'passenger',
      component: PassengerComponent,
    },
    {
      path: 'ticket',
      component: TicketComponent,
    },
  ],
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class TablesRoutingModule { }

export const routedComponents = [
  PassengerTicketComponent,
  PassengerComponent,
  TicketComponent
];
