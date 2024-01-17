import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {DistanceFareComponent} from './distance-fare.component';
import {ListDistanceFareComponent} from './list-distance-fare/list-distance-fare.component';
import {AddDistanceFareComponent} from './add-distance-fare/add-distance-fare.component';
import {EditDistanceFareComponent} from './edit-distance-fare/edit-distance-fare.component';


const routes: Routes = [{
  path: '',
  component: DistanceFareComponent,
  children: [
    {
      path: '',
      component: ListDistanceFareComponent,
    },
    {
      path: 'add',
      component: AddDistanceFareComponent,
    },
    {
      path: ':id/edit',
      component: EditDistanceFareComponent,
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

export class DistanceFareRoutingModule { }
