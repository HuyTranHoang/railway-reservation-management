import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {PaymentAndCancellationComponent} from './payment-and-cancellation.component';
import {PaymentComponent} from './payment/payment.component';
import {CancellationComponent} from './cancellation/cancellation.component';
import {CancellationRuleComponent} from './cancellation-rule/cancellation-rule.component';
import {CancellationRuleModule} from './cancellation-rule/cancellation-rule.module';
import {PaymentAndCancellationRoutingModule} from './payment-and-cancellation-routing.module';
import {SharedModule} from '../shared/shared.module';
import {AddPaymentComponent} from './payment/add-payment/add-payment.component';
import {EditPaymentComponent} from './payment/edit-payment/edit-payment.component';
import {ShowPaymentComponent} from './payment/show-payment/show-payment.component';
import {ConfirmDeletePaymentComponent} from './payment/confirm-delete-payment/confirm-delete-payment.component';
import { CancellationModule } from './cancellation/cancellation.module';

@NgModule({
  declarations: [
    PaymentAndCancellationComponent,

    PaymentComponent,
    CancellationComponent,
    CancellationRuleComponent,

    AddPaymentComponent,
    EditPaymentComponent,
    ShowPaymentComponent,
    ConfirmDeletePaymentComponent,
  ],
  imports: [
    CommonModule,
    SharedModule,

    CancellationRuleModule,
    CancellationModule,

    PaymentAndCancellationRoutingModule,
  ],
})
export class PaymentAndCancellationModule {
}
