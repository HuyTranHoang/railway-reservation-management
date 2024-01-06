import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ScheduleComponent } from './schedule/schedule.component';
import { TicketpriceComponent } from './ticketprice/ticketprice.component';
import { ScheduleTicketPriceRoute, routedComponents } from './schedule-ticketprice-routing.module';



@NgModule({
  declarations: [
    routedComponents
  ],
  imports: [
    ScheduleTicketPriceRoute
  ]
})
export class ScheduleTicketpriceModule { }
