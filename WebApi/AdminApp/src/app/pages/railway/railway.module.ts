import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TrainstationComponent } from './trainstation/trainstation.component';
import { TraincompanyComponent } from './traincompany/traincompany.component';
import { RaiwayRoute, routedComponents } from './railway-routing.module';
import { NbContextMenuModule } from '@nebular/theme/components/context-menu/context-menu.module';



@NgModule({
  declarations: [
      ...routedComponents
  ],
  imports: [
    RaiwayRoute,
  ]
})
export class RailwayModule { }
