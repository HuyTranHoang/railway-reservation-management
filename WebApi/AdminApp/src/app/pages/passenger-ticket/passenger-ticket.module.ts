import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PassengerComponent } from './passenger/passenger.component';
import { TicketComponent } from './ticket/ticket.component';
import {  TablesRoutingModule, routedComponents } from './passenger-ticket-routing.module';
import { FsIconComponent } from '../tables/tree-grid/tree-grid.component';
import { NbInputModule } from '@nebular/theme/components/input/input.module';
import { NbIconModule } from '@nebular/theme/components/icon/icon.module';
import { NbTreeGridModule } from '@nebular/theme/components/tree-grid/tree-grid.module';
import { NbCardModule } from '@nebular/theme/components/card/card.module';
import { ThemeModule } from '../../@theme/theme.module';



@NgModule({
  declarations: [
    ...routedComponents,
    

  ],
  imports: [
    TablesRoutingModule,
    ThemeModule
  ]
})
export class PassengerTicketModule { }
