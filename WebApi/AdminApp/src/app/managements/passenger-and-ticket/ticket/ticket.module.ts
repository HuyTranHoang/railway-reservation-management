import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AddTicketComponent } from './add-ticket/add-ticket.component';
import { ShowTicketComponent } from './show-ticket/show-ticket.component';
import { EditTicketComponent } from './edit-ticket/edit-ticket.component';
import { ConfirmDeleteTicketComponent } from './confirm-delete-ticket/confirm-delete-ticket.component';



@NgModule({
  declarations: [
    AddTicketComponent,
    ShowTicketComponent,
    EditTicketComponent,
    ConfirmDeleteTicketComponent
  ],
  imports: [
    CommonModule
  ]
})
export class TicketModule { }
