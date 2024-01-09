import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {RailwayComponent} from '../railway/railway.component';
import {CarriageComponent} from './carriage/carriage.component';
import {CarriageTypeComponent} from './carriage-type/carriage-type.component';
import {CompartmentComponent} from './compartment/compartment.component';
import {TrainComponent} from './train/train.component';
import {AddCarriageTypeComponent} from './carriage-type/add-carriage-type/add-carriage-type.component';
import {EditCarriageTypeComponent} from './carriage-type/edit-carriage-type/edit-carriage-type.component';

const routes: Routes = [{
  path: '',
  component: RailwayComponent,
  children: [
    {
      path: 'train',
      component: TrainComponent,
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
