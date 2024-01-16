import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {ScheduleComponent} from './schedule.component';
import {ListScheduleComponent} from './list-schedule/list-schedule.component';
import {AddScheduleComponent} from './add-schedule/add-schedule.component';
import {EditScheduleComponent} from './edit-schedule/edit-schedule.component';


const routes: Routes = [{
  path: '',
  component: ScheduleComponent,
  children: [
    {
      path: '',
      component: ListScheduleComponent,
    },
    {
      path: 'add',
      component: AddScheduleComponent,
    },
    {
      path: ':id/edit',
      component: EditScheduleComponent,
    },
  ],
}];

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes),
  ],
  exports: [RouterModule],
})
export class ScheduleRoutingModule {
}
