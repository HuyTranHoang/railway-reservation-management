import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {RailwayComponent} from '../railway/railway.component';
import {CarriageComponent} from './carriage/carriage.component';
import {CompartmentComponent} from './compartment/compartment.component';
import { AddTrainComponent } from './train/add-train/add-train.component';
import { TrainComponent } from './train/train.component';
import { ListTrainComponent } from './train/list-train/list-train.component';
import { EditTrainComponent } from './train/edit-train/edit-train.component';
const routes: Routes = [{
  path: '',
  component: RailwayComponent,
  children: [
    {
      path: 'train',
      component: TrainComponent,
      children: [
        {
          path: '',
          component: ListTrainComponent,
        },
        {
          path: 'add',
          component: AddTrainComponent,
        },
        {
          path: ':id/edit',
          component: EditTrainComponent,
        },
      ],
    },
    {
      path: 'carriage',
      loadChildren: () => import('./carriage/carriage.module')
        .then(m => m.CarriageModule),
    },
    {
      path: 'carriage-type',
      loadChildren: () => import('./carriage-type/carriage-type.module')
        .then(m => m.CarriageTypeModule),
    },
    {
      path: 'compartment',
      component: CompartmentComponent,
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
export class TrainAndCarriageRoutingModule {
}
