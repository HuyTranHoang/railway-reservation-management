import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {RouterLink} from '@angular/router';
import {NbAutocompleteModule, NbOptionModule, NbSelectModule} from '@nebular/theme';
import {SharedModule} from '../../shared/shared.module';
import {ListSeatComponent} from './list-seat/list-seat.component';
import {AddSeatComponent} from './add-seat/add-seat.component';
import {EditSeatComponent} from './edit-seat/edit-seat.component';
import {ConfirmDeleteSeatComponent} from './confirm-delete-seat/confirm-delete-seat.component';
import {ShowSeatComponent} from './show-seat/show-seat.component';


@NgModule({
  declarations: [
    ListSeatComponent,
    AddSeatComponent,
    EditSeatComponent,
    ConfirmDeleteSeatComponent,
    ShowSeatComponent,
  ],
  imports: [
    CommonModule,
    SharedModule,
    NbOptionModule,
    NbSelectModule,
    NbAutocompleteModule,
    RouterLink,
  ]
})
export class SeatModule { }
