import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {CarriageComponent} from './carriage.component';
import {ListCarriageComponent} from './list-carriage/list-carriage.component';
import {AddCarriageComponent} from './add-carriage/add-carriage.component';
import {EditCarriageComponent} from './edit-carriage/edit-carriage.component';

const routes: Routes = [{
  path: '',
  component: CarriageComponent,
  children: [
    {
      path: '',
      component: ListCarriageComponent,
    },
    {
      path: 'add',
      component: AddCarriageComponent,
    },
    {
      path: ':id/edit',
      component: EditCarriageComponent,
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
export class CarriageRoutingModule {
}
