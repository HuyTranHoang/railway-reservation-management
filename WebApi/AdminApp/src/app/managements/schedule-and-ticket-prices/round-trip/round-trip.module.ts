import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {ListRoundTripComponent} from './list-round-trip/list-round-trip.component';
import {AddRoundTripComponent} from './add-round-trip/add-round-trip.component';
import {EditRoundTripComponent} from './edit-round-trip/edit-round-trip.component';
import {ShowRoundTripComponent} from './show-round-trip/show-round-trip.component';
import {ConfirmDeleteRoundTripComponent} from './confirm-delete-round-trip/confirm-delete-round-trip.component';
import {SharedModule} from '../../shared/shared.module';
import {RoundTripRoutingModule} from './round-trip-routing.module';


@NgModule({
  declarations: [
    ListRoundTripComponent,
    AddRoundTripComponent,
    EditRoundTripComponent,
    ShowRoundTripComponent,
    ConfirmDeleteRoundTripComponent,
  ],
  imports: [
    CommonModule,
    SharedModule,
    RoundTripRoutingModule,
  ],
})
export class RoundTripModule {
}
