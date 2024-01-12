import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AddTrainComponent } from './add-train/add-train.component';
import { EditTrainComponent } from './edit-train/edit-train.component';
import { ConfirmDeleteTrainComponent } from './confirm-delete-train/confirm-delete-train.component';
import { ListTrainComponent } from './list-train/list-train.component';
import { NbAlertModule, NbOptionModule, NbSelectModule } from '@nebular/theme';
import { SharedModule } from '../../shared/shared.module';
import { RouterLink } from '@angular/router';



@NgModule({
  declarations: [
    AddTrainComponent,
    EditTrainComponent,
    ConfirmDeleteTrainComponent,
    ListTrainComponent,
  ],
  imports: [
    CommonModule,
    SharedModule,
    NbOptionModule,
    NbSelectModule,
    NbAlertModule,
    RouterLink,
  ]
})
export class TrainModule { }
