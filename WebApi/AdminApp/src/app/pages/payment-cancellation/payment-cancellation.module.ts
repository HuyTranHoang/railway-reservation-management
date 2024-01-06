import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PaymentComponent } from './payment/payment.component';
import { CancellationComponent } from './cancellation/cancellation.component';
import { PaymentCancellationRouteModule, routedComponents } from './payment-cancellation-routing.module';



@NgModule({
  declarations: [
    ...routedComponents,
  ],
  imports: [
    PaymentCancellationRouteModule
  ]
})
export class PaymentCancellationModule { }
