import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserRoutingModule } from './user-routing.module';
import { SharedModule } from '../shared/shared.module';
import { UserProfileComponent } from './user-profile/user-profile.component';
import { ReactiveFormsModule } from '@angular/forms';
import { UserBookingComponent } from './user-booking/user-booking.component';
import { HeaderComponent } from './user-booking/header/header.component';


@NgModule({
  declarations: [
    UserProfileComponent,
    UserBookingComponent,
    HeaderComponent,
  ],
  imports: [
    CommonModule,
    UserRoutingModule,
    SharedModule,
    ReactiveFormsModule,
  ]
})
export class UserModule { }
