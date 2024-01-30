import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserRoutingModule } from './user-routing.module';
import { SharedModule } from '../shared/shared.module';
import { UserProfileComponent } from './user-profile/user-profile.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms'
import { UserBookingHistoryComponent } from './user-booking-history/user-booking-history.component';
import { ConfirmCanceledTicketComponent } from './confirm-canceled-ticket/confirm-canceled-ticket.component';
import { TicketTableComponent } from './user-booking-history/ticket-table/ticket-table.component';
import { TicketCodeFilterPipe } from './user-booking-history/ticket-table/ticket-code-filter.pipe';


@NgModule({
  declarations: [
    UserProfileComponent,
    UserBookingHistoryComponent,
    ConfirmCanceledTicketComponent,
    TicketTableComponent,
    TicketCodeFilterPipe,
  ],
  imports: [
    CommonModule,
    UserRoutingModule,
    SharedModule,
    FormsModule,
    ReactiveFormsModule,
  ]
})
export class UserModule { }
