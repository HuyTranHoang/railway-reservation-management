import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {PaymentAndCancellationComponent} from './payment-and-cancellation.component';
import {PaymentComponent} from './payment/payment.component';
import {CancellationComponent} from './cancellation/cancellation.component';
import {AddPaymentComponent} from './payment/add-payment/add-payment.component';
import {EditPaymentComponent} from './payment/edit-payment/edit-payment.component';

const routes: Routes = [{
  path: '',
  component: PaymentAndCancellationComponent,
  children: [
    {
      path: 'payment',
      component: PaymentComponent,
    },
    {
      path: 'payment/add',
      component: AddPaymentComponent,
    },
    {
      path: 'payment/:id/edit',
      component: EditPaymentComponent,
    },
    {
      path: 'cancellation',
      component: CancellationComponent,
    },
    {
      path: 'cancellation-rule',
      loadChildren: () => import('./cancellation-rule/cancellation-rule.module')
        .then(m => m.CancellationRuleModule),
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
export class PaymentAndCancellationRoutingModule { }
