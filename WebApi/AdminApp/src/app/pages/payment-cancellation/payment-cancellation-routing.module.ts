import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { PaymentCancellationComponent } from './payment-cancellation.component';
import { PaymentComponent } from './payment/payment.component';
import { CancellationComponent } from './cancellation/cancellation.component';



const routes: Routes = [{
  path: '',
  component: PaymentCancellationComponent,
  children: [
    {
      path: 'payment',
      component: PaymentComponent,
    },
    {
      path: 'cancellation',
      component: CancellationComponent,
    },
  ],
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class PaymentCancellationRouteModule { }

export const routedComponents = [
  PaymentComponent,
  CancellationComponent,
  PaymentCancellationComponent
];
