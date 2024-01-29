import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserRoutingModule } from './user-routing.module';
import { SharedModule } from '../shared/shared.module';
import { UserProfileComponent } from './user-profile/user-profile.component';
import { ReactiveFormsModule } from '@angular/forms';
import { UserBookingHistoryComponent } from './user-booking-history/user-booking-history.component';
import { ConfirmCanceledTicketComponent } from './confirm-canceled-ticket/confirm-canceled-ticket.component';
import { TicketTableComponent } from './user-booking-history/ticket-table/ticket-table.component';


@NgModule({
  declarations: [
    UserProfileComponent,
    UserBookingHistoryComponent,
    ConfirmCanceledTicketComponent,
    TicketTableComponent,
  ],
  imports: [
    CommonModule,
    UserRoutingModule,
    SharedModule,
    ReactiveFormsModule,
  ]
})
export class UserModule { }
