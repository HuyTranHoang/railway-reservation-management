import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {AddTrainComponent} from './add-train/add-train.component';
import {EditTrainComponent} from './edit-train/edit-train.component';
import {ConfirmDeleteTrainComponent} from './confirm-delete-train/confirm-delete-train.component';
import {ListTrainComponent} from './list-train/list-train.component';
import {NbAutocompleteModule, NbOptionModule, NbSelectModule} from '@nebular/theme';
import {SharedModule} from '../../shared/shared.module';
import {RouterLink} from '@angular/router';
import {ShowTrainComponent} from './show-train/show-train.component';


@NgModule({
  declarations: [
    AddTrainComponent,
    EditTrainComponent,
    ConfirmDeleteTrainComponent,
    ListTrainComponent,
    ShowTrainComponent,
  ],
  imports: [
    CommonModule,
    SharedModule,
    NbOptionModule,
    NbSelectModule,
    NbAutocompleteModule,
    RouterLink,
  ],
})
export class TrainModule { }
