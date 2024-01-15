import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {ScheduleAndTicketPricesComponent} from './schedule-and-ticket-prices.component';
import {ScheduleComponent} from './schedule/schedule.component';
import { ListScheduleComponent } from './schedule/list-schedule/list-schedule.component';
import { AddScheduleComponent } from './schedule/add-schedule/add-schedule.component';


const routes: Routes = [{
  path: '',
  component: ScheduleAndTicketPricesComponent,
  children: [
    {
      path: 'schedule',
      component: ScheduleComponent,
      children : [
        {
          path : '',
          component : ListScheduleComponent,
        },
        {
          path : 'add',
          component : AddScheduleComponent,
        }
      ]
    },
    {
      path: 'distance-fare',
      loadChildren: () => import('./distance-fare/distance-fare.module')
        .then(m => m.DistanceFareModule),
    },
    {
      path: 'round-trip',
      loadChildren: () => import('./round-trip/round-trip.module')
        .then(m => m.RoundTripModule),
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

export class ScheduleAndTicketPricesRoutingModule {
}
