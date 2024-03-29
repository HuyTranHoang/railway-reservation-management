import { NgModule } from '@angular/core'
import { RouterModule, Routes } from '@angular/router'
import { HomeComponent } from './home/home.component'
import { TestErrorComponent } from './core/test-error/test-error.component'
import { NotFoundComponent } from './core/not-found/not-found.component'
import { ServerErrorComponent } from './core/server-error/server-error.component'
import { HelpComponent } from './help/help.component'
import { AboutComponent } from './about/about.component'
import { PaymentSuccessComponent } from './payment-success/payment-success.component'

const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'user', loadChildren: () => import('./user/user.module').then(mod => mod.UserModule) },
  { path: 'auth', loadChildren: () => import('./auth/auth.module').then(mod => mod.AuthModule) },
  { path: 'booking', loadChildren: () => import('./booking/booking.module').then(mod => mod.BookingModule) },
  { path: 'test-error', component: TestErrorComponent },
  { path: 'not-found', component: NotFoundComponent },
  { path: 'server-error', component: ServerErrorComponent },
  { path: 'help', component: HelpComponent },
  { path: 'about', component: AboutComponent },
  { path: 'payment-success', component: PaymentSuccessComponent },
  {
    path: 'management',
    loadChildren: () => import('./manage-booking/manage-booking.module').then(mod => mod.ManageBookingModule)
  },
  { path: 'not-implemented', redirectTo: '', pathMatch: 'full' },
  { path: '**', redirectTo: 'not-found', pathMatch: 'full' }
]

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
