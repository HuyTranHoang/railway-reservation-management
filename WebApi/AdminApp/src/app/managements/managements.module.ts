import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {ManagementsComponent} from './managements.component';
import {ThemeModule} from '../@theme/theme.module';
import {NbIconLibraries, NbInputModule, NbMenuModule} from '@nebular/theme';
import {ManagementsRoutingModule} from './managements-routing.module';
import {HttpClientModule} from '@angular/common/http';
import {SharedModule} from './shared/shared.module';
import {DailyTransactionComponent} from './daily-transaction/daily-transaction.component';
import { BarchartjsComponent } from './dashboard/barchartjs/barchartjs.component';
import {ChartModule} from 'angular2-chartjs';
import {DashboardComponent} from './dashboard/dashboard.component';
import { ShowExportExcelComponent } from './daily-transaction/show-export-excel/show-export-excel.component';


@NgModule({
  declarations: [
    ManagementsComponent,
    DashboardComponent,
    DailyTransactionComponent,
    BarchartjsComponent,
    ShowExportExcelComponent,
  ],
  imports: [
    CommonModule,
    HttpClientModule,
    ManagementsRoutingModule,
    ThemeModule,
    NbMenuModule,
    NbInputModule,

    SharedModule,
    ChartModule,
  ],
})
export class ManagementsModule {
  constructor(iconsLibrary: NbIconLibraries) {
    iconsLibrary.registerFontPack('font-awesome', { iconClassPrefix: 'fa', packClass: 'fa' });
  }
}
