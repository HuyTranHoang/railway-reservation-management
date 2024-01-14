import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { NbOptionModule, NbSelectModule, NbAutocompleteModule } from '@nebular/theme';
import { SharedModule } from '../../shared/shared.module';
import { ListSeatComponent } from './list-seat/list-seat.component';
import { AddSeatComponent } from './add-seat/add-seat.component';



@NgModule({
  declarations: [
    ListSeatComponent,
    AddSeatComponent,
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
