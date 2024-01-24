import { NgModule } from '@angular/core'
import { RouterModule, Routes } from '@angular/router'
import { BookingComponent } from './booking.component'
import { DepartureComponent } from './departure/departure.component'
import { PassengersComponent } from './passengers/passengers.component'
import { SeatSelectionComponent } from './seat-selection/seat-selection.component'
import { PaymentComponent } from './payment/payment.component'

const routes: Routes = [
  {
    path: '',
    component: BookingComponent,
    children: [
      { path: '', component: DepartureComponent },
      { path: 'seat-selection', component: SeatSelectionComponent },
      { path: 'passengers', component: PassengersComponent },
      { path: 'payment', component: PaymentComponent },
      { path: '**', redirectTo: '' }
    ]
  }
]

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [
    RouterModule
  ]
})
export class BookingRoutingModule {}
