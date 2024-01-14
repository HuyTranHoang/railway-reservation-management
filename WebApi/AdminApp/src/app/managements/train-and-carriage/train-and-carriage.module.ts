import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {TrainAndCarriageComponent} from './train-and-carriage.component';
import {TrainComponent} from './train/train.component';
import {TrainAndCarriageRoutingModule} from './train-and-carriage-routing.module';
import {SharedModule} from '../shared/shared.module';
import {CarriageTypeComponent} from './carriage-type/carriage-type.component';
import {CarriageComponent} from './carriage/carriage.component';
import {CompartmentComponent} from './compartment/compartment.component';
import {TrainModule} from './train/train.module';
import {CarriageTypeModule} from './carriage-type/carriage-type.module';

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
    SharedModule,
    // Child Module
    TrainModule,
    CarriageTypeModule,
    // Routing
    TrainAndCarriageRoutingModule,
  ],
})
export class TrainAndCarriageModule { }
