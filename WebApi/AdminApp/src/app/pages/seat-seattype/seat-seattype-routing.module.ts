import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { SeatSeattypeComponent } from './seat-seattype.component';
import { SeatComponent } from './seat/seat.component';
import { SeattypeComponent } from './seattype/seattype.component';





const routes: Routes = [{
  path: '',
  component: SeatSeattypeComponent,
  children: [
    {
      path: 'seat',
      component: SeatComponent,
    },
    {
      path: 'seattype',
      component: SeattypeComponent,
    },
  ],
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class SeatSeatTypeRoute { }

export const routedComponents = [
  SeatSeattypeComponent,
  SeatComponent,
  SeattypeComponent
];
