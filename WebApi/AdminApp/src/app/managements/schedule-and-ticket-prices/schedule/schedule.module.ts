import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ListScheduleComponent } from './list-schedule/list-schedule.component';
import { RouterLink } from '@angular/router';
import { SharedModule } from '../../shared/shared.module';
import { ConfirmDeleteScheduleComponent } from './confirm-delete-schedule/confirm-delete-schedule.component';
import { AddScheduleComponent } from './add-schedule/add-schedule.component';
import { ShowScheduleComponent } from './show-schedule/show-schedule.component';
import { EditScheduleComponent } from './edit-schedule/edit-schedule.component';


@NgModule({
  declarations: [
    ListScheduleComponent,
    ConfirmDeleteScheduleComponent,
    AddScheduleComponent,
    ShowScheduleComponent,
    EditScheduleComponent,
  ],
  imports: [
    CommonModule,
    SharedModule,
    RouterLink,
  ]
})
export class ScheduleModule { }
