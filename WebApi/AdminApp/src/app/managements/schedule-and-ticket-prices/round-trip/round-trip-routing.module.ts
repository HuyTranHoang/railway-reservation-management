import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {CarriageTypeComponent} from '../../train-and-carriage/carriage-type/carriage-type.component';
import {ListRoundTripComponent} from './list-round-trip/list-round-trip.component';
import {AddRoundTripComponent} from './add-round-trip/add-round-trip.component';
import {EditRoundTripComponent} from './edit-round-trip/edit-round-trip.component';


const routes: Routes = [{
  path: '',
  component: CarriageTypeComponent,
  children: [
    {
      path: '',
      component: ListRoundTripComponent,
    },
    {
      path: 'add',
      component: AddRoundTripComponent,
    },
    {
      path: ':id/edit',
      component: EditRoundTripComponent,
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

export class RoundTripRoutingModule {
}
