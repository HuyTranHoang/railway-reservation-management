import {NgModule} from '@angular/core'
import {LoginComponent} from './login/login.component'
import {RegisterComponent} from './register/register.component'
import {RouterModule, Routes} from '@angular/router'
import { ConfirmEmailComponent } from './confirm-email/confirm-email.component'
import { SendEmailComponent } from './send-email/send-email.component'
import { ResetPasswordComponent } from './reset-password/reset-password.component'

const routes: Routes = [
  {path: 'login', component: LoginComponent},
  {path: 'register', component: RegisterComponent},
  {path: 'confirm-email', component: ConfirmEmailComponent},
  {path: 'send-email/:mode', component: SendEmailComponent},
  {path: 'reset-password', component: ResetPasswordComponent}
]

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ShopRoutingModule {
}
