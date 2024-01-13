import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {CarriageTypeComponent} from './carriage-type.component';
import {ListCarriageTypeComponent} from './list-carriage-type/list-carriage-type.component';
import {AddCarriageTypeComponent} from './add-carriage-type/add-carriage-type.component';
import {EditCarriageTypeComponent} from './edit-carriage-type/edit-carriage-type.component';


const routes: Routes = [{
  path: '',
  component: CarriageTypeComponent,
  children: [
    {
      path: '',
      component: ListCarriageTypeComponent,
    },
    {
      path: 'add',
      component: AddCarriageTypeComponent,
    },
    {
      path: ':id/edit',
      component: EditCarriageTypeComponent,
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
export class CarriageTypeRoutingModule {
}
