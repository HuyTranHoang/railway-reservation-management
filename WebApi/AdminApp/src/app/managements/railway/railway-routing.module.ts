import { NgModule } from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {RailwayComponent} from './railway.component';
import {TrainStationComponent} from './train-station/train-station.component';
import {TrainCompanyComponent} from './train-company/train-company.component';

const routes: Routes = [{
  path: '',
  component: RailwayComponent,
  children: [
    {
      path: 'train-station',
      component: TrainStationComponent,
    },
    {
      path: 'train-company',
      component: TrainCompanyComponent,
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

export class RailwayRoutingModule { }
