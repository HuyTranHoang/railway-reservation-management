import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {PaymentAndCancellationComponent} from './payment-and-cancellation.component';
import {PaymentComponent} from './payment/payment.component';
import {CancellationComponent} from './cancellation/cancellation.component';
import {CancellationRuleComponent} from './cancellation-rule/cancellation-rule.component';
import {PaymentAndCancellationRoutingModule} from './payment-and-cancellation-routing.module';

@NgModule({
  declarations: [
    PaymentAndCancellationComponent,
    PaymentComponent,
    CancellationComponent,
    CancellationRuleComponent,
  ],
  imports: [
    CommonModule,
    PaymentAndCancellationRoutingModule,
  ],
})
export class PaymentAndCancellationModule {
}
