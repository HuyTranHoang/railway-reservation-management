import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { RailwayComponent } from './railway.component';
import { TraincompanyComponent } from './traincompany/traincompany.component';
import { TrainstationComponent } from './trainstation/trainstation.component';



const routes: Routes = [{
  path: '',
  component: RailwayComponent,
  children: [
    {
      path: 'traincompany',
      component: TraincompanyComponent,
    },
    {
      path: 'trainstation',
      component: TrainstationComponent,
    },
  ],
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class RaiwayRoute { }

export const routedComponents = [
  RailwayComponent,
  TraincompanyComponent,
  TrainstationComponent
];
