import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TrainAndCarriageComponent } from './train-and-carriage.component';
import { TrainComponent } from './train/train.component';
import { TrainAndCarriageRoutingModule } from './train-and-carriage-routing.module';
import { NbOptionModule, NbSelectModule } from '@nebular/theme';
import { SharedModule } from '../shared/shared.module';
import { AddCarriageTypeComponent } from './carriage-type/add-carriage-type/add-carriage-type.component';
import { CarriageTypeComponent } from './carriage-type/carriage-type.component';
import { ConfirmDeleteCarriageTypeComponent } from './carriage-type/confirm-delete-carriage-type/confirm-delete-carriage-type.component';
import { EditCarriageTypeComponent } from './carriage-type/edit-carriage-type/edit-carriage-type.component';
import { ShowCarriageTypeComponent } from './carriage-type/show-carriage-type/show-carriage-type.component';
import { CarriageComponent } from './carriage/carriage.component';
import { CompartmentComponent } from './compartment/compartment.component';
import { TrainModule } from './train/train.module';



@NgModule({
  declarations: [
    TrainAndCarriageComponent,
    TrainComponent,
    CarriageComponent,
    CarriageTypeComponent,
    CompartmentComponent,
    AddCarriageTypeComponent,
    EditCarriageTypeComponent,
    ConfirmDeleteCarriageTypeComponent,
    ShowCarriageTypeComponent,
  ],
  imports: [
    CommonModule,
    SharedModule,
    TrainAndCarriageRoutingModule,
    NbOptionModule,
    NbSelectModule,
    TrainModule,
  ],
})
export class TrainAndCarriageModule { }
