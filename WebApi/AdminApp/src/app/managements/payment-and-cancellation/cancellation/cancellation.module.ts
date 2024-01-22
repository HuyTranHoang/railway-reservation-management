import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {ListCancellationComponent} from './list-cancellation/list-cancellation.component';
import {SharedModule} from '../../shared/shared.module';
import {CancellationRoutingModule} from './cancellation-routing.module';
import {AddCancellationComponent} from './add-cancellation/add-cancellation.component';
import {EditCancellationComponent} from './edit-cancellation/edit-cancellation.component';
import {ShowCancellationComponent} from './show-cancellation/show-cancellation.component';
import {ConfirmDeleteCancellationComponent} from './confirm-delete-cancellation/confirm-delete-cancellation.component';


@NgModule({
  declarations: [
    ListCancellationComponent,
    AddCancellationComponent,
    EditCancellationComponent,
    ShowCancellationComponent,
    ConfirmDeleteCancellationComponent,
  ],
  imports: [
    CommonModule,
    SharedModule,

    CancellationRoutingModule,
  ]
})
export class CancellationModule {}
