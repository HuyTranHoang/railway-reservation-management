import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RailwayComponent } from './railway.component';
import { TrainStationComponent } from './train-station/train-station.component';
import { TrainCompanyComponent } from './train-company/train-company.component';
import {RailwayRoutingModule} from './railway-routing.module';



@NgModule({
  declarations: [
    RailwayComponent,
    TrainStationComponent,
    TrainCompanyComponent,
  ],
  imports: [
    CommonModule,
    RailwayRoutingModule,
  ],
})
export class RailwayModule { }
