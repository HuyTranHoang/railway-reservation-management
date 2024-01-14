import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ListScheduleComponent } from './list-schedule/list-schedule.component';
import { RouterLink } from '@angular/router';
import { NbOptionModule, NbSelectModule, NbAutocompleteModule } from '@nebular/theme';
import { SharedModule } from '../../shared/shared.module';
import { ConfirmDeleteScheduleComponent } from './confirm-delete-schedule/confirm-delete-schedule.component';



@NgModule({
  declarations: [
    ListScheduleComponent,
    ConfirmDeleteScheduleComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    NbOptionModule,
    NbSelectModule,
    NbAutocompleteModule,
    RouterLink,
  ]
})
export class ScheduleModule { }
