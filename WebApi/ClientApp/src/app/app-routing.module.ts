import {NgModule} from '@angular/core'
import {RouterModule, Routes} from '@angular/router'
import {HomeComponent} from './home/home.component'
import {TestErrorComponent} from './core/test-error/test-error.component'
import {NotFoundComponent} from './core/not-found/not-found.component'
import {ServerErrorComponent} from './core/server-error/server-error.component'
import {BookingTrainComponent} from './booking-train/booking-train.component'

const routes: Routes = [
  {path: '', component: HomeComponent},
  {path: 'auth', loadChildren: () => import('./auth/auth.module').then(mod => mod.AuthModule)},
  {path: 'test-error', component: TestErrorComponent},
  {path: 'not-found', component: NotFoundComponent},
  {path: 'server-error', component: ServerErrorComponent},
  {path: 'not-implemented', redirectTo:'', pathMatch: 'full'},
  {path: 'booking-train', component: BookingTrainComponent}
]

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
