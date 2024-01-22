import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {UserComponent} from './user.component';
import {ListUserComponent} from './list-user/list-user.component';
import {UserRoutingModule} from './user-routing.module';
import {SharedModule} from '../shared/shared.module';
import { ConfirmLockUserComponent } from './confirm-lock-user/confirm-lock-user.component';


@NgModule({
  declarations: [
    UserComponent,
    ListUserComponent,
    ConfirmLockUserComponent,
  ],
  imports: [
    CommonModule,
    UserRoutingModule,
    SharedModule,
  ],
})
export class UserModule {
}
