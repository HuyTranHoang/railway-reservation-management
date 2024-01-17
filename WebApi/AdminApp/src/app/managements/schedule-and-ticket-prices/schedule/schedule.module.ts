import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {ListScheduleComponent} from './list-schedule/list-schedule.component';
import {SharedModule} from '../../shared/shared.module';
import {ConfirmDeleteScheduleComponent} from './confirm-delete-schedule/confirm-delete-schedule.component';
import {AddScheduleComponent} from './add-schedule/add-schedule.component';
import {ShowScheduleComponent} from './show-schedule/show-schedule.component';
import {EditScheduleComponent} from './edit-schedule/edit-schedule.component';
import {ScheduleRoutingModule} from './schedule-routing.module';
import {ThemeModule} from '../../../@theme/theme.module';


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

    ScheduleRoutingModule,
    ThemeModule,
  ],
})
export class ScheduleModule {
}
