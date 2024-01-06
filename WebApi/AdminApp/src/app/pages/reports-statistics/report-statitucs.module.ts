import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReportComponent } from './report/report.component';
import { StatiticsComponent } from './statitics/statitics.component';
import { ReportStatiticRoute, routedComponents } from './report-statitics-routing.module';



@NgModule({
  declarations: [
    ...routedComponents
  ],
  imports: [
    ReportStatiticRoute
  ]
})
export class ReportStatitucsModule { }
