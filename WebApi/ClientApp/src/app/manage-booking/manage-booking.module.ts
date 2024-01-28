import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../shared/shared.module';
import { ManageRoutingModule } from './manage-routing.module';

import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { ShowTicketComponent } from './show-ticket/show-ticket.component';
import { ManagementBookingComponent } from './management-booking/management-booking.component';
import { DateDiffPipe } from './date-diff.pipe'



@NgModule({
  declarations: [
    ManagementBookingComponent,
    ShowTicketComponent,
    DateDiffPipe
  ],
  imports: [
    CommonModule,
    ManageRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    SharedModule,
    RouterLink,

  ]
})
export class ManageBookingModule { }
