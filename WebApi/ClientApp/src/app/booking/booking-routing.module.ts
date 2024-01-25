import { NgModule } from '@angular/core'
import { RouterModule, Routes } from '@angular/router'
import { BookingComponent } from './booking.component'
import { DepartureComponent } from './departure/departure.component'
import { PassengersComponent } from './passengers/passengers.component'
import { SeatSelectionComponent } from './seat-selection/seat-selection.component'
import { PaymentComponent } from './payment/payment.component'
import { AuthorizationGuard } from '../core/guards/authorization.guard'

const routes: Routes = [
  {
    path: '',
    component: BookingComponent,
    children: [
      { path: '', component: DepartureComponent },
      {
        path: 'seat-selection',
        runGuardsAndResolvers: 'always',
        canActivate: [AuthorizationGuard],
        component: SeatSelectionComponent
      },
      {
        path: 'passengers',
        runGuardsAndResolvers: 'always',
        canActivate: [AuthorizationGuard],
        component: PassengersComponent
      },
      {
        path: 'payment',
        runGuardsAndResolvers: 'always',
        canActivate: [AuthorizationGuard],
        component: PaymentComponent
      },
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
