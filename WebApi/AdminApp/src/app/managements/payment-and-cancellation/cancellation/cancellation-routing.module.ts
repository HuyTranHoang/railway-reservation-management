import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {CancellationComponent} from './cancellation.component';
import {ListCancellationComponent} from './list-cancellation/list-cancellation.component';
import {AddCancellationComponent} from './add-cancellation/add-cancellation.component';
import {EditCancellationComponent} from './edit-cancellation/edit-cancellation.component';


const routes: Routes = [{
  path: '',
  component: CancellationComponent,
  children: [
    {
      path: '',
      component: ListCancellationComponent,
    },
    {
      path: 'add',
      component: AddCancellationComponent,
    },
    {
      path: ':id/edit',
      component: EditCancellationComponent,
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

export class CancellationRoutingModule { }
