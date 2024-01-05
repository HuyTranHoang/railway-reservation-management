import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router'
import { BookingComponent } from './booking.component'
import { DepartureComponent } from './departure/departure.component'
import { SharedModule } from '../shared/shared.module'


const routes: Routes = [
  {
    path: '',
    component: BookingComponent,
    children: [
      { path: '', component: DepartureComponent },
    ]
  },
];

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes),
  ],
  exports: [
    RouterModule
  ]
})
export class BookingRoutingModule { }
