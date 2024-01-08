import {NgModule} from '@angular/core'
import {LoginComponent} from './login/login.component'
import {RegisterComponent} from './register/register.component'
import {RouterModule, Routes} from '@angular/router'
import { ConfirmEmailComponent } from './confirm-email/confirm-email.component'
import { SendEmailComponent } from './send-email/send-email.component'

const routes: Routes = [
  {path: 'login', component: LoginComponent},
  {path: 'register', component: RegisterComponent},
  {path: 'confirm-email', component: ConfirmEmailComponent},
  {path: 'send-email/:mode', component: SendEmailComponent}
]

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ShopRoutingModule {
}
