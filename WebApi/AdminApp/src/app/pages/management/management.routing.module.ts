import { RouterModule, Routes } from "@angular/router";
import { ManagementComponent } from "./management.component";
import { TicketmanagementComponent } from "./ticketmanagement/ticketmanagement.component";
import { NgModule } from "@angular/core";
import { ScheduleComponent } from "./schedule/schedule.component";
import { TrainComponent } from "./train/train.component";

const routes: Routes = [{
    path: '',
    component: ManagementComponent,
    children: [
      {
        path: 'ticketmanagement',
        component: TicketmanagementComponent,
      },
      {
        path: 'schedulemanagement',
        component: ScheduleComponent,
      },
      {
        path: 'trainmanagement',
        component: TrainComponent,
      },
    ],
  }];
  
  @NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
  })
  export class ManagementRoutingModule { }
  
  export const routedComponents = [
    ManagementComponent,
    TicketmanagementComponent,
    ScheduleComponent
  ];
  