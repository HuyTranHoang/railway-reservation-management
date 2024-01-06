import { NgModule } from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {PaymentAndCancellationComponent} from './payment-and-cancellation.component';
import {PaymentComponent} from './payment/payment.component';
import {CancellationComponent} from './cancellation/cancellation.component';
import {CancellationRuleComponent} from './cancellation-rule/cancellation-rule.component';



const routes: Routes = [{
  path: '',
  component: PaymentAndCancellationComponent,
  children: [
    {
      path: 'payment',
      component: PaymentComponent,
    },
    {
      path: 'cancellation',
      component: CancellationComponent,
    },
    {
      path: 'cancellation-rule',
      component: CancellationRuleComponent,
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
