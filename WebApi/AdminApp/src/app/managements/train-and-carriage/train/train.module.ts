import { CommonModule } from '@angular/common';
import { AddTrainComponent } from './add-train/add-train.component';
import { NgModule } from '@angular/core';
import { SharedModule } from '../../shared/shared.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ListTrainComponent } from './list-train/list-train.component';
import { RouterModule } from '@angular/router';
import { EditTrainComponent } from './edit-train/edit-train.component';
import { ConfirmDeleteTrainComponent } from './confirm-delete-train/confirm-delete-train.component';
import { ShowTrainComponent } from './show-train/show-train.component';



@NgModule({
  declarations: [
    AddTrainComponent,
    ListTrainComponent,
    EditTrainComponent,
    ConfirmDeleteTrainComponent,
    ShowTrainComponent,
  ],
  imports: [
    CommonModule,
    SharedModule,
    FormsModule,
    RouterModule,
    ReactiveFormsModule,
  ]
})
export class TrainModule { }
