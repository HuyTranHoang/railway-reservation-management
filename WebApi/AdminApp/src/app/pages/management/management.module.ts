import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ManagementRoutingModule, routedComponents } from './management.routing.module';
import { FsIconComponent } from '../tables/tree-grid/tree-grid.component';
import { Ng2SmartTableModule } from 'ng2-smart-table/lib/ng2-smart-table.module';
import { ThemeModule } from '../../@theme/theme.module';
import { NbInputModule } from '@nebular/theme/components/input/input.module';
import { NbIconModule } from '@nebular/theme/components/icon/icon.module';
import { NbTreeGridModule } from '@nebular/theme/components/tree-grid/tree-grid.module';
import { NbCardModule } from '@nebular/theme/components/card/card.module';



@NgModule({
  declarations: [
    ...routedComponents,
  
  ],
  imports: [
    ManagementRoutingModule,
  ]
})
export class ManagementModule { }
