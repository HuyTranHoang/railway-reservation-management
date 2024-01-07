import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {ManagementsComponent} from './managements.component';
import {DashboardComponent} from './dashboard/dashboard.component';
import {NotFoundComponent} from '../pages/miscellaneous/not-found/not-found.component';

const routes: Routes = [{
  path: '',
  component: ManagementsComponent,
  children: [
    {
      path: 'dashboard',
      component: DashboardComponent,
    },
    {
      path: '',
      redirectTo: 'dashboard',
      pathMatch: 'full',
    },
    {
      path: 'railway',
      loadChildren: () => import('./railway/railway.module')
        .then(m => m.RailwayModule),
    },
    {
      path: 'train-and-carriage',
      loadChildren: () => import('./train-and-carriage/train-and-carriage.module')
        .then(m => m.TrainAndCarriageModule),
    },
    {
      path: 'seat-and-seat-type',
      loadChildren: () => import('./seat-and-seat-type/seat-and-seat-type.module')
        .then(m => m.SeatAndSeatTypeModule),
    },
    {
      path: 'schedule-and-ticket-prices',
      loadChildren: () => import('./schedule-and-ticket-prices/schedule-and-ticket-prices.module')
        .then(m => m.ScheduleAndTicketPricesModule),
    },
    {
      path: 'passenger-and-ticket',
      loadChildren: () => import('./passenger-and-ticket/passenger-and-ticket.module')
        .then(m => m.PassengerAndTicketModule),
    },
    {
      path: 'payment-and-cancellation',
      loadChildren: () => import('./payment-and-cancellation/payment-and-cancellation.module')
        .then(m => m.PaymentAndCancellationModule),
    },
    {
      path: '**',
      component: NotFoundComponent,
    },
  ],
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})

export class ManagementsRoutingModule {
}
