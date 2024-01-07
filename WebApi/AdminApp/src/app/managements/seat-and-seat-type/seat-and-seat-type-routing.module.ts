import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {SeatAndSeatTypeComponent} from './seat-and-seat-type.component';
import {SeatComponent} from './seat/seat.component';
import {SeatTypeComponent} from './seat-type/seat-type.component';


const routes: Routes = [{
  path: '',
  component: SeatAndSeatTypeComponent,
  children: [
    {
      path: 'seat',
      component: SeatComponent,
    },
    {
      path: 'seat-type',
      component: SeatTypeComponent,
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
