import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {RailwayComponent} from '../railway/railway.component';
import {CarriageComponent} from './carriage/carriage.component';
import {CarriageTypeComponent} from './carriage-type/carriage-type.component';
import {CompartmentComponent} from './compartment/compartment.component';
import {AddCarriageTypeComponent} from './carriage-type/add-carriage-type/add-carriage-type.component';
import {EditCarriageTypeComponent} from './carriage-type/edit-carriage-type/edit-carriage-type.component';
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
      ]
    },
    {
      path: 'carriage',
      component: CarriageComponent,
    },
    {
      path: 'carriage-type',
      component: CarriageTypeComponent,
    },
    {
      path: 'carriage-type/add',
      component: AddCarriageTypeComponent,
    },
    {
      path: 'carriage-type/:id/edit',
      component: EditCarriageTypeComponent,
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
