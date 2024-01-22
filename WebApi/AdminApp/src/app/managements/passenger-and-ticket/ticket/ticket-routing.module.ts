import { NgModule } from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {TicketComponent} from './ticket.component';
import {ListTicketComponent} from './list-ticket/list-ticket.component';
import {AddTicketComponent} from './add-ticket/add-ticket.component';
import {EditTicketComponent} from './edit-ticket/edit-ticket.component';

const routes: Routes = [{
  path: '',
  component: TicketComponent,
  children: [
    {
      path: '',
      component: ListTicketComponent,
    },
    {
      path: 'add',
      component: AddTicketComponent,
    },
    {
      path: ':id/edit',
      component: EditTicketComponent,
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

export class TicketRoutingModule { }
