import { RouterModule, Routes } from '@angular/router';
import { NgModule } from '@angular/core';

import { PagesComponent } from './pages.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { ECommerceComponent } from './e-commerce/e-commerce.component';
import { NotFoundComponent } from './miscellaneous/not-found/not-found.component';
import { SystemComponent } from './system/system.component';

const routes: Routes = [{
  path: '',
  component: PagesComponent,
  children: [
    {
      path: 'dashboard',
      component: ECommerceComponent,
    },
    {
      path: 'iot-dashboard',
      component: DashboardComponent,
    },
    {
      path: 'layout',
      loadChildren: () => import('./layout/layout.module')
        .then(m => m.LayoutModule),
    },
    {
      path: 'forms',
      loadChildren: () => import('./forms/forms.module')
        .then(m => m.FormsModule),
    },
    {
      path: 'ui-features',
      loadChildren: () => import('./ui-features/ui-features.module')
        .then(m => m.UiFeaturesModule),
    },
    {
      path: 'modal-overlays',
      loadChildren: () => import('./modal-overlays/modal-overlays.module')
        .then(m => m.ModalOverlaysModule),
    },
    {
      path: 'extra-components',
      loadChildren: () => import('./extra-components/extra-components.module')
        .then(m => m.ExtraComponentsModule),
    },
    {
      path: 'maps',
      loadChildren: () => import('./maps/maps.module')
        .then(m => m.MapsModule),
    },
    {
      path: 'charts',
      loadChildren: () => import('./charts/charts.module')
        .then(m => m.ChartsModule),
    },
    {
      path: 'editors',
      loadChildren: () => import('./editors/editors.module')
        .then(m => m.EditorsModule),
    },
    {
      path: 'tables',
      loadChildren: () => import('./tables/tables.module')
        .then(m => m.TablesModule),
    },
    {
      path: 'passenger-ticket',
      loadChildren: () => import('./passenger-ticket/passenger-ticket.module')
        .then(m => m.PassengerTicketModule),
    },
    {
      path: 'payment-cancellation',
      loadChildren: () => import('./payment-cancellation/payment-cancellation.module')
        .then(m => m.PaymentCancellationModule),
    },
    {
      path: 'report-statitics',
      loadChildren: () => import('./reports-statistics/report-statitucs.module')
        .then(m => m.ReportStatitucsModule),
    },
    {
      path: 'schedule-ticketprice',
      loadChildren: () => import('./schedule-ticketprice/schedule-ticketprice.module')
        .then(m => m.ScheduleTicketpriceModule),
    },
    {
      path: 'railway',
      loadChildren: () => import('./railway/railway.module')
        .then(m => m.RailwayModule),
    },
    {
      path: 'system',
      component: SystemComponent,
    },
    {
      path: 'train-carriage',
      loadChildren: () => import('./train-carriage/train-carriage.module')
        .then(m => m.TrainCarriageModule),
    },
    {
      path: 'seat-seattype',
      loadChildren: () => import('./seat-seattype/seat-seattype.module')
        .then(m => m.SeatSeattypeModule),
    },
    {
      path: 'miscellaneous',
      loadChildren: () => import('./miscellaneous/miscellaneous.module')
        .then(m => m.MiscellaneousModule),
    },
    {
      path: '',
      redirectTo: 'dashboard',
      pathMatch: 'full',
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
export class PagesRoutingModule {
}
