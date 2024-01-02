import {NgModule} from '@angular/core'
import {CommonModule} from '@angular/common'
import {NavBarComponent} from './nav-bar/nav-bar.component'
import {ServerErrorComponent} from './server-error/server-error.component'
import {NotFoundComponent} from './not-found/not-found.component'
import {TestErrorComponent} from './test-error/test-error.component'
import {RouterModule} from '@angular/router'


@NgModule({
  declarations: [
    NavBarComponent,
    ServerErrorComponent,
    NotFoundComponent,
    TestErrorComponent
  ],
  imports: [
    CommonModule,
    RouterModule
  ],
  exports: [
    NavBarComponent
  ]
})
export class CoreModule {
}
