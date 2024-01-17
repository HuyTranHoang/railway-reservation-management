import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {CompartmentComponent} from './compartment.component';
import {ListCompartmentComponent} from './list-compartment/list-compartment.component';
import {AddCompartmentComponent} from './add-compartment/add-compartment.component';
import {EditCompartmentComponent} from './edit-compartment/edit-compartment.component';

const routes: Routes = [{
  path: '',
  component: CompartmentComponent,
  children: [
    {
      path: '',
      component: ListCompartmentComponent,
    },
    {
      path: 'add',
      component: AddCompartmentComponent,
    },
    {
      path: ':id/edit',
      component: EditCompartmentComponent,
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

export class CompartmentRoutingModule { }
