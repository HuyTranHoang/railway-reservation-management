import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {RailwayComponent} from './railway.component';
import {TrainStationComponent} from './train-station/train-station.component';
import {TrainCompanyComponent} from './train-company/train-company.component';
import {RailwayRoutingModule} from './railway-routing.module';
import {AddTrainCompanyComponent} from './train-company/add-train-company/add-train-company.component';
import {SharedModule} from '../shared/shared.module';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {EditTrainCompanyComponent} from './train-company/edit-train-company/edit-train-company.component';
import {ShowTrainCompanyComponent} from './train-company/show-train-company/show-train-company.component';
import {
  ConfirmDeleteTrainCompanyComponent,
} from './train-company/confirm-delete-train-company/confirm-delete-train-company.component';
import {ShowTrainStationComponent} from './train-station/show-train-station/show-train-station.component';
import {AddTrainStationComponent} from './train-station/add-train-station/add-train-station.component';
import {DeleteTrainStationComponent} from './train-station/delete-train-station/delete-train-station.component';
import {EditTrainStationComponent} from './train-station/edit-train-station/edit-train-station.component';


@NgModule({
  declarations: [
    RailwayComponent,
    TrainStationComponent,
    TrainCompanyComponent,
    AddTrainCompanyComponent,
    EditTrainCompanyComponent,
    ShowTrainCompanyComponent,
    ConfirmDeleteTrainCompanyComponent,
    ShowTrainStationComponent,
    AddTrainStationComponent,
    DeleteTrainStationComponent,
    EditTrainStationComponent,
  ],
  imports: [
    CommonModule,
    RailwayRoutingModule,
    FormsModule,
    SharedModule,
    ReactiveFormsModule,
  ],
})
export class RailwayModule { }
