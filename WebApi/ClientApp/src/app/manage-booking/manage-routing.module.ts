import { Component, NgModule } from '@angular/core'
import { RouterModule, Routes } from '@angular/router'

import { ManagementBookingComponent } from './management-booking/management-booking.component'
import { ShowTicketComponent } from './show-ticket/show-ticket.component'



const routes: Routes = [
  {
   path : '',
   component : ManagementBookingComponent,
  },
  {
    path : 'ticket',
    component : ShowTicketComponent
  }

]

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes),
  ],
  exports: [RouterModule],
})
export class ManageRoutingModule {}
