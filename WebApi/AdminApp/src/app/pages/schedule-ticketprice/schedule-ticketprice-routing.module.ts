import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ScheduleTicketpriceComponent } from './schedule-ticketprice.component';
import { ScheduleComponent } from './schedule/schedule.component';
import { TicketComponent } from '../passenger-ticket/ticket/ticket.component';
import { TicketpriceComponent } from './ticketprice/ticketprice.component';




const routes: Routes = [{
  path: '',
  component: ScheduleTicketpriceComponent,
  children: [
    {
      path: 'schedule',
      component: ScheduleComponent,
    },
    {
      path: 'ticketprice',
      component: TicketComponent,
    },
  ],
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ScheduleTicketPriceRoute { }

export const routedComponents = [
  ScheduleTicketpriceComponent,
  ScheduleComponent,
  TicketpriceComponent
];
