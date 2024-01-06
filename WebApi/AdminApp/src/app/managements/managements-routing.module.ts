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
