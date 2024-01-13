import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {ScheduleAndTicketPricesComponent} from './schedule-and-ticket-prices.component';
import {ScheduleComponent} from './schedule/schedule.component';
import {DistanceFareComponent} from './distance-fare/distance-fare.component';
import { AddCancellationRuleComponent } from '../payment-and-cancellation/cancellation-rule/add-cancellation-rule/add-cancellation-rule.component';
import { EditCancellationRuleComponent } from '../payment-and-cancellation/cancellation-rule/edit-cancellation-rule/edit-cancellation-rule.component';



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
