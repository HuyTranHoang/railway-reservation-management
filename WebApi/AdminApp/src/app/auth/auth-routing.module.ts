import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {LoginComponent} from './login/login.component';
import {NotFoundComponent} from '../pages/miscellaneous/not-found/not-found.component';
import {AuthComponent} from './auth.component';
import { SendEmailComponent } from './send-email/send-email.component';
import { ResetPasswordComponent } from './reset-password/reset-password.component';



const routes: Routes = [{
  path: '',
  component: AuthComponent,
  children: [
    {
      path : '',
      component : LoginComponent,
    },
    {
      path: 'login',
      component: LoginComponent,
    },
    {
      path: 'send-email/:mode',
      component: SendEmailComponent
    },
    {
      path: 'reset-password',
      component: ResetPasswordComponent
    },
    {
      path: '**',
      component: NotFoundComponent,
    },
  ],
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})

export class AuthRoutingModule {
}
