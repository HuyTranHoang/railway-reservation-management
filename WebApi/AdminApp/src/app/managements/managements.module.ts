import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {ManagementsComponent} from './managements.component';
import {ThemeModule} from '../@theme/theme.module';
import {NbIconLibraries, NbMenuModule} from '@nebular/theme';
import {DashboardModule} from '../pages/dashboard/dashboard.module';
import {ECommerceModule} from '../pages/e-commerce/e-commerce.module';
import {MiscellaneousModule} from '../pages/miscellaneous/miscellaneous.module';
import {ManagementsRoutingModule} from './managements-routing.module';
import {DashboardComponent} from './dashboard/dashboard.component';
import {HttpClientModule} from '@angular/common/http';


@NgModule({
  declarations: [
    ManagementsComponent,
    DashboardComponent,
  ],
  imports: [
    CommonModule,
    HttpClientModule,
    ManagementsRoutingModule,
    ThemeModule,
    NbMenuModule,
    DashboardModule,
    ECommerceModule,
    MiscellaneousModule,
  ],
})
export class ManagementsModule {
  constructor(iconsLibrary: NbIconLibraries) {
    iconsLibrary.registerFontPack('font-awesome', { iconClassPrefix: 'fa', packClass: 'fa' });
  }
}
