import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {TrainAndCarriageComponent} from './train-and-carriage.component';
import { TrainComponent } from './train/train.component';
import { CarriageComponent } from './carriage/carriage.component';
import { CarriageTypeComponent } from './carriage-type/carriage-type.component';
import { CompartmentComponent } from './compartment/compartment.component';
import {TrainAndCarriageRoutingModule} from './train-and-carriage-routing.module';


@NgModule({
  declarations: [
    TrainAndCarriageComponent,
    TrainComponent,
    CarriageComponent,
    CarriageTypeComponent,
    CompartmentComponent,
  ],
  imports: [
    CommonModule,
    TrainAndCarriageRoutingModule,
  ],
})
export class TrainAndCarriageModule {
}
