import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {AddTrainComponent} from './train/add-train/add-train.component';
import {TrainComponent} from './train/train.component';
import {ListTrainComponent} from './train/list-train/list-train.component';
import {EditTrainComponent} from './train/edit-train/edit-train.component';
import {TrainAndCarriageComponent} from './train-and-carriage.component';


const routes: Routes = [{
  path: '',
  component: TrainAndCarriageComponent,
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
      loadChildren: () => import('./compartment/compartment.module')
        .then(m => m.CompartmentModule),
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
