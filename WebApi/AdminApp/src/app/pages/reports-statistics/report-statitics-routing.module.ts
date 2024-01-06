import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ReportStatitucsModule } from './report-statitucs.module';
import { ReportComponent } from './report/report.component';
import { StatiticsComponent } from './statitics/statitics.component';
import { ReportsStatisticsComponent } from './reports-statistics.component';




const routes: Routes = [{
  path: '',
  component: ReportsStatisticsComponent,
  children: [
    {
      path: 'report',
      component: ReportComponent,
    },
    {
      path: 'statitics',
      component: StatiticsComponent,
    },
  ],
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ReportStatiticRoute { }

export const routedComponents = [
  ReportsStatisticsComponent,
  StatiticsComponent,
  ReportComponent
];
