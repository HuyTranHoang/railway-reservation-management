import { BrowserAnimationsModule } from '@angular/platform-browser/animations'
import { BrowserModule } from '@angular/platform-browser'
import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core'
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http'
import { HomeComponent } from './home/home.component'
import { LoadingInterceptor } from './core/interceptors/loading.interceptor'
import { ErrorInterceptor } from './core/interceptors/error.interceptor'
import { CoreModule } from './core/core.module'
import { AppComponent } from './app.component'
import { AppRoutingModule } from './app-routing.module'
import { SharedModule } from './shared/shared.module'
import { JwtInterceptor } from './core/interceptors/jwt.interceptor'
import { HelpComponent } from './help/help.component'
import { AboutComponent } from './about/about.component'
import { BookingModule } from './booking/booking.module';
import { SliderComponent } from './home/slider/slider.component'
import { FormsModule } from '@angular/forms';
import { GalleryComponent } from './home/gallery/gallery.component';
import { UserComponent } from './user/user.component';
import { HelpContactComponent } from './help/help-contact/help-contact.component';
import { HelpFaqsComponent } from './help/help-faqs/help-faqs.component';
import { ManageBookingModule } from './manage-booking/manage-booking.module';
import { PaymentSuccessComponent } from './payment-success/payment-success.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    HelpComponent,
    AboutComponent,
    SliderComponent,
    GalleryComponent,
    UserComponent,
    HelpContactComponent,
    HelpFaqsComponent,
    PaymentSuccessComponent,
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    FormsModule,
    HttpClientModule,
    AppRoutingModule,
    CoreModule,
    SharedModule,
    BookingModule,
    ManageBookingModule
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: LoadingInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true }
  ],
  bootstrap: [AppComponent],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class AppModule {
}
