import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {UserComponent} from './user.component';
import {ListUserComponent} from './list-user/list-user.component';


const routes: Routes = [{
  path: '',
  component: UserComponent,
  children: [
    {
      path: '',
      component: ListUserComponent,
    },
  ],
}];

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes),
  ],
  exports: [RouterModule],
})
export class UserRoutingModule {
}
