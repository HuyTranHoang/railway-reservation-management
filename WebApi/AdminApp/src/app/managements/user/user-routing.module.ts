import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {UserComponent} from './user.component';
import {ListUserComponent} from './list-user/list-user.component';
import {AddUserComponent} from './add-user/add-user.component';
import {EditUserComponent} from './edit-user/edit-user.component';


const routes: Routes = [{
  path: '',
  component: UserComponent,
  children: [
    {
      path: '',
      component: ListUserComponent,
    },
    {
      path: 'add',
      component: AddUserComponent,
    },
    {
      path: ':id/edit',
      component: EditUserComponent,
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
