import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {ListDistanceFareComponent} from './list-distance-fare/list-distance-fare.component';
import {SharedModule} from '../../shared/shared.module';
import {DistanceFareRoutingModule} from './distance-fare-routing.module';
import {AddDistanceFareComponent} from './add-distance-fare/add-distance-fare.component';
import {EditDistanceFareComponent} from './edit-distance-fare/edit-distance-fare.component';
import {ShowDistanceFareComponent} from './show-distance-fare/show-distance-fare.component';
import {
  ConfirmDeleteDistanceFareComponent,
} from './confirm-delete-distance-fare/confirm-delete-distance-fare.component';


@NgModule({
  declarations: [
    ListDistanceFareComponent,
    AddDistanceFareComponent,
    EditDistanceFareComponent,
    ShowDistanceFareComponent,
    ConfirmDeleteDistanceFareComponent,
  ],
  imports: [
    CommonModule,
    SharedModule,

    DistanceFareRoutingModule,
  ],
})
export class DistanceFareModule {
}
