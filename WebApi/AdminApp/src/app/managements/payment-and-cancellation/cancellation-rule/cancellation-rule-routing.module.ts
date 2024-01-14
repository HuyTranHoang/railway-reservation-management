import { NgModule } from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { CancellationRuleComponent } from './cancellation-rule.component';
import { ListCancellationRuleComponent } from './list-cancellation-rule/list-cancellation-rule.component';
import { AddCancellationRuleComponent } from './add-cancellation-rule/add-cancellation-rule.component';
import { EditCancellationRuleComponent } from './edit-cancellation-rule/edit-cancellation-rule.component';



const routes: Routes = [{
  path: '',
  component: CancellationRuleComponent,
  children: [
    {
      path: '',
      component: ListCancellationRuleComponent,
    },
    {
      path: 'add',
      component: AddCancellationRuleComponent,
    },
    {
      path: ':id/edit',
      component: EditCancellationRuleComponent,
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

export class CancellationRuleRoutingModule { }
