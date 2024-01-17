import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {ScheduleAndTicketPricesComponent} from './schedule-and-ticket-prices.component';

const routes: Routes = [{
  path: '',
  component: ScheduleAndTicketPricesComponent,
  children: [
    {
      path: 'schedule',
      loadChildren: () => import('./schedule/schedule.module')
        .then(m => m.ScheduleModule),
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
