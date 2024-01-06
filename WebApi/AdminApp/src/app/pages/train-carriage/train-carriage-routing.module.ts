import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { TrainCarriageComponent } from './train-carriage.component';
import { TrainComponent } from './train/train.component';
import { CarriageComponent } from './carriage/carriage.component';






const routes: Routes = [{
  path: '',
  component: TrainCarriageComponent,
  children: [
    {
      path: 'train',
      component: TrainComponent,
    },
    {
      path: 'carriage',
      component: CarriageComponent,
    },
  ],
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class TrainCarriageRoute { }

export const routedComponents = [
    TrainCarriageComponent,
    TrainComponent,
    CarriageComponent
];
