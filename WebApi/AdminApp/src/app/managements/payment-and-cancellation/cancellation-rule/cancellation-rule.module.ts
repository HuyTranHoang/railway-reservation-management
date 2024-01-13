import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ListCancellationRuleComponent } from './list-cancellation-rule/list-cancellation-rule.component';
import { SharedModule } from '../../shared/shared.module';
import { CancellationRuleRoutingModule } from './cancellation-rule-routing.module';
import { AddCancellationRuleComponent } from './add-cancellation-rule/add-cancellation-rule.component';
import { EditCancellationRuleComponent } from './edit-cancellation-rule/edit-cancellation-rule.component';
import { ShowCancellationRuleComponent } from './show-cancellation-rule/show-cancellation-rule.component';
import { ConfirmDeleteCancellationRuleComponent } from './confirm-delete-cancellation-rule/confirm-delete-cancellation-rule.component';




@NgModule({
  declarations: [
    ListCancellationRuleComponent,
    AddCancellationRuleComponent,
    EditCancellationRuleComponent,
    ShowCancellationRuleComponent,
    ConfirmDeleteCancellationRuleComponent,
  ],
  imports: [
    CommonModule,
    SharedModule,

    CancellationRuleRoutingModule,
  ]
})
export class CancellationRuleModule {
}
