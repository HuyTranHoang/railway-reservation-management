import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UserProfileComponent } from './user-profile/user-profile.component';
import { UserBookingHistoryComponent } from './user-booking-history/user-booking-history.component';
import { AuthorizationGuard } from '../core/guards/authorization.guard'

const routes: Routes = [
  {
    path: 'profile',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthorizationGuard],
    component: UserProfileComponent,
  },
  {
    path: 'booking',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthorizationGuard],
    component : UserBookingHistoryComponent,
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class UserRoutingModule { }
