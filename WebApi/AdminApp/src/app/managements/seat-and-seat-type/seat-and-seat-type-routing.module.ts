import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {SeatAndSeatTypeComponent} from './seat-and-seat-type.component';
import {SeatComponent} from './seat/seat.component';
import {SeatTypeComponent} from './seat-type/seat-type.component';
import {AddSeatTypeComponent} from './seat-type/add-seat-type/add-seat-type.component';
import {EditSeatTypeComponent} from './seat-type/edit-seat-type/edit-seat-type.component';
import { ListSeatComponent } from './seat/list-seat/list-seat.component';
import { AddSeatComponent } from './seat/add-seat/add-seat.component';
import { EditSeatComponent } from './seat/edit-seat/edit-seat.component';


const routes: Routes = [{
  path: '',
  component: SeatAndSeatTypeComponent,
  children: [
    {
      path: 'seat',
      component: SeatComponent,
      children : [
        {
          path: '',
          component: ListSeatComponent,
        },
        {
          path : 'add',
          component: AddSeatComponent,
        },
        {
          path: ':id/edit',
          component: EditSeatComponent,
        },
      ]
    },
    {
      path: 'seat-type',
      component: SeatTypeComponent,
    },
    {
      path: 'seat-type/add',
      component: AddSeatTypeComponent,
    },
    {
      path: 'seat-type/edit/:id',
      component: EditSeatTypeComponent,
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

export class SeatAndSeatTypeRoutingModule {
}
