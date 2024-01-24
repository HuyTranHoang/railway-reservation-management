import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {LoginComponent} from './login/login.component';
import {AuthRoutingModule} from './auth-routing.module';
import { AuthComponent } from './auth.component';
import {HttpClientModule} from '@angular/common/http';
import {NbInputModule, NbLayoutModule} from '@nebular/theme';
import {SharedModule} from '../managements/shared/shared.module';
import { SendEmailComponent } from './send-email/send-email.component';
import { ResetPasswordComponent } from './reset-password/reset-password.component';

@NgModule({
  declarations: [
    LoginComponent,
    AuthComponent,
    SendEmailComponent,
    ResetPasswordComponent,
  ],
  imports: [
    CommonModule,
    HttpClientModule,
    AuthRoutingModule,
    NbInputModule,
    NbLayoutModule,
    SharedModule,
  ],
})

export class AuthModule {
}
