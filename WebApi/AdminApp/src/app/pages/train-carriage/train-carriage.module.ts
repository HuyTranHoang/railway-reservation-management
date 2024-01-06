import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TrainComponent } from './train/train.component';
import { CarriageComponent } from './carriage/carriage.component';
import { TrainCarriageRoute, routedComponents } from './train-carriage-routing.module';



@NgModule({
  declarations: [
      ...routedComponents
  ],
  imports: [
    TrainCarriageRoute
  ]
})
export class TrainCarriageModule { }
