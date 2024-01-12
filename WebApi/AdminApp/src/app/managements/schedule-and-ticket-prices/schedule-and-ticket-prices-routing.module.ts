import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {ScheduleAndTicketPricesComponent} from './schedule-and-ticket-prices.component';
import {ScheduleComponent} from './schedule/schedule.component';
import {DistanceFareComponent} from './distance-fare/distance-fare.component';
import {RoundTripComponent} from './round-trip/round-trip.component';
import { AddRoundTripComponent } from './round-trip/add-round-trip/add-round-trip.component';
import { EditRoundTripComponent } from './round-trip/edit-round-trip/edit-round-trip.component';


const routes: Routes = [{
  path: '',
  component: ScheduleAndTicketPricesComponent,
  children: [
    {
      path: 'schedule',
      component: ScheduleComponent,
    },
    {
      path: 'distance-fare',
      component: DistanceFareComponent,
    },
    {
      path: 'round-trip',
      component: RoundTripComponent,
    },
    {
      path: 'round-trip/add',
      component: AddRoundTripComponent,
    },
    {
      path: 'round-trip/:id/edit',
      component: EditRoundTripComponent,
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
