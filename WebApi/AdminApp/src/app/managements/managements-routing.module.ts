import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {ManagementsComponent} from './managements.component';
import {RailwayComponent} from './railway/railway.component';

const routes: Routes = [{
  path: '',
  component: ManagementsComponent,
  children: [
    {
      path: 'railway',
      component: RailwayComponent,
    },
  ],
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})

export class ManagementsRoutingModule {
}
