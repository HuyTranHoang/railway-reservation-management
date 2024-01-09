import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {TrainAndCarriageComponent} from './train-and-carriage.component';
import { TrainComponent } from './train/train.component';
import { CarriageComponent } from './carriage/carriage.component';
import { CarriageTypeComponent } from './carriage-type/carriage-type.component';
import { CompartmentComponent } from './compartment/compartment.component';
import {TrainAndCarriageRoutingModule} from './train-and-carriage-routing.module';
import {
  NbButtonModule,
  NbCardModule,
  NbCheckboxModule,
  NbIconModule,
  NbInputModule,
  NbRadioModule,
} from '@nebular/theme';
import { AddCarriageTypeComponent } from './carriage-type/add-carriage-type/add-carriage-type.component';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import { ShowCarriageTypeComponent } from './carriage-type/show-carriage-type/show-carriage-type.component';


@NgModule({
  declarations: [
    TrainAndCarriageComponent,
    TrainComponent,
    CarriageComponent,
    CarriageTypeComponent,
    CompartmentComponent,
    AddCarriageTypeComponent,
    ShowCarriageTypeComponent,
  ],
  imports: [
    CommonModule,
    TrainAndCarriageRoutingModule,
    NbButtonModule,
    FormsModule,
    ReactiveFormsModule,
    NbCardModule,
    NbCheckboxModule,
    NbInputModule,
    NbIconModule,
    NbRadioModule,
  ],
})
export class TrainAndCarriageModule {
}
