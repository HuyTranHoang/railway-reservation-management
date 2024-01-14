import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {RailwayComponent} from './railway.component';
import {TrainStationComponent} from './train-station/train-station.component';
import {TrainCompanyComponent} from './train-company/train-company.component';
import {AddTrainCompanyComponent} from './train-company/add-train-company/add-train-company.component';
import {EditTrainCompanyComponent} from './train-company/edit-train-company/edit-train-company.component';
import {AddTrainStationComponent} from './train-station/add-train-station/add-train-station.component';
import {EditTrainStationComponent} from './train-station/edit-train-station/edit-train-station.component';
import {DeleteTrainStationComponent} from './train-station/delete-train-station/delete-train-station.component';

const routes: Routes = [{
  path: '',
  component: RailwayComponent,
  children: [
    {
      path: 'train-station',
      component: TrainStationComponent,
    },
    {
      path: 'train-station/add',
      component: AddTrainStationComponent,
    },
    {
      path: 'train-station/edit/:id',
      component: EditTrainStationComponent,
    },
    {
      path: 'train-station/delete/:id',
      component: DeleteTrainStationComponent,
    },
    {
      path: 'train-company',
      component: TrainCompanyComponent,
    },
    {
      path: 'train-company/add',
      component: AddTrainCompanyComponent,
    },
    {
      path: 'train-company/:id/edit',
      component: EditTrainCompanyComponent,
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
