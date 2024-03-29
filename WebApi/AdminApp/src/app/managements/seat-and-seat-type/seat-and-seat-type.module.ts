import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {SeatComponent} from './seat/seat.component';
import {SeatTypeComponent} from './seat-type/seat-type.component';
import {SeatAndSeatTypeComponent} from './seat-and-seat-type.component';
import {SeatAndSeatTypeRoutingModule} from './seat-and-seat-type-routing.module';
import {SharedModule} from '../shared/shared.module';
import {AddSeatTypeComponent} from './seat-type/add-seat-type/add-seat-type.component';
import {ShowSeatTypeComponent} from './seat-type/show-seat-type/show-seat-type.component';
import {EditSeatTypeComponent} from './seat-type/edit-seat-type/edit-seat-type.component';
import {ConfirmDeleteSeatTypeComponent} from './seat-type/confirm-delete-seat-type/confirm-delete-seat-type.component';
import {SeatModule} from './seat/seat.module';


@NgModule({
  declarations: [
    SeatComponent,
    SeatTypeComponent,
    SeatAndSeatTypeComponent,
    AddSeatTypeComponent,
    ShowSeatTypeComponent,
    EditSeatTypeComponent,
    ConfirmDeleteSeatTypeComponent,
  ],
  imports: [
    CommonModule,
    SharedModule,
    // Child Module
    SeatModule,

    // Routing
    SeatAndSeatTypeRoutingModule,
  ],
})
export class SeatAndSeatTypeModule {
}
