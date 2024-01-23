import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {PassengerAndTicketComponent} from './passenger-and-ticket.component';
import {PassengerComponent} from './passenger/passenger.component';
import {AddPassengerComponent} from './passenger/add-passenger/add-passenger.component';
import {EditPassengerComponent} from './passenger/edit-passenger/edit-passenger.component';


const routes: Routes = [{
  path: '',
  component: PassengerAndTicketComponent,
  children: [
    {
      path: 'passenger',
      component: PassengerComponent,
    },
    {
      path: 'passenger/add',
      component: AddPassengerComponent,
    },
    {
      path: 'passenger/:id/edit',
      component: EditPassengerComponent,
    },
    {
      path: 'ticket',
      loadChildren: () => import('./ticket/ticket.module')
        .then(m => m.TicketModule),
    },
  ],
}];

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes),
  ],
  exports: [RouterModule],
})

export class PassengerAndTicketRoutingModule { }
