import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {RouterModule, Routes} from '@angular/router';
import {RailwayComponent} from '../railway/railway.component';
import {TrainStationComponent} from '../railway/train-station/train-station.component';
import {TrainCompanyComponent} from '../railway/train-company/train-company.component';
import {PassengerAndTicketComponent} from './passenger-and-ticket.component';
import {PassengerComponent} from './passenger/passenger.component';
import {TicketComponent} from './ticket/ticket.component';



const routes: Routes = [{
  path: '',
  component: PassengerAndTicketComponent,
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
  declarations: [],
  imports: [
    RouterModule.forChild(routes),
  ],
  exports: [RouterModule],
})

export class PassengerAndTicketRoutingModule { }
