import {NgModule} from '@angular/core'
import {RouterModule, Routes} from '@angular/router'
import {HomeComponent} from './home/home.component'
import {TestErrorComponent} from './core/test-error/test-error.component'
import {NotFoundComponent} from './core/not-found/not-found.component'
import {ServerErrorComponent} from './core/server-error/server-error.component'
import {BookingTrainComponent} from './booking-train/booking-train.component'
import { AuthorizationGuard } from './core/guards/authorization.guard'
import { ContactComponent } from './contact/contact.component'
import { AboutComponent } from './about/about.component'
import { FaqsComponent } from './faqs/faqs.component'

const routes: Routes = [
  {path: '', component: HomeComponent},
  {path: 'user', loadChildren: () => import('./user/user.module').then(mod => mod.UserModule)},
  {path: 'auth', loadChildren: () => import('./auth/auth.module').then(mod => mod.AuthModule)},
  {path: 'booking', loadChildren: () => import('./booking/booking.module').then(mod => mod.BookingModule)},
  {path: 'test-error', component: TestErrorComponent},
  {path: 'not-found', component: NotFoundComponent},
  {path: 'server-error', component: ServerErrorComponent},
  {path: 'contact', component: ContactComponent},
  {path: 'about', component: AboutComponent},
  {path: 'faqs', component: FaqsComponent},
  {path: 'not-implemented', redirectTo:'', pathMatch: 'full'},
  {
    path: 'booking-train',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthorizationGuard],
    component: BookingTrainComponent
  },
  {path: '**', component: NotFoundComponent, pathMatch: 'full'}
]

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
