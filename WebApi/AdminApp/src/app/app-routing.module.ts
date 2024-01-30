import {ExtraOptions, RouterModule, Routes} from '@angular/router';
import {NgModule} from '@angular/core';
import {AuthorizationGuard} from './guards/authorization.guard';

export const routes: Routes = [
  {
    path: 'managements',
    loadChildren: () => import('./managements/managements.module')
      .then(m => m.ManagementsModule),
    runGuardsAndResolvers: 'always',
    canActivate: [AuthorizationGuard],
  },
  {
    path: 'auth',
    loadChildren: () => import('./auth/auth.module')
      .then(m => m.AuthModule),
  },
  {path: '', redirectTo: 'auth', pathMatch: 'full'},
  {path: '**', redirectTo: 'auth'},
];

const config: ExtraOptions = {
  useHash: false,
};

@NgModule({
  imports: [RouterModule.forRoot(routes, config)],
  exports: [RouterModule],
})
export class AppRoutingModule {
}
