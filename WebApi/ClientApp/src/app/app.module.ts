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
import { BookingTrainComponent } from './booking-train/booking-train.component'
import { SharedModule } from './shared/shared.module'
import { JwtInterceptor } from './core/interceptors/jwt.interceptor'
import { ContactComponent } from './contact/contact.component'
import { AboutComponent } from './about/about.component'
import { FaqsComponent } from './faqs/faqs.component'
import { BookingModule } from './booking/booking.module';
import { SliderComponent } from './home/slider/slider.component'
import { FormsModule } from '@angular/forms';
import { GalleryComponent } from './home/gallery/gallery.component';
import { UserComponent } from './user/user.component'

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    BookingTrainComponent,
    ContactComponent,
    AboutComponent,
    FaqsComponent,
    SliderComponent,
    GalleryComponent,
    UserComponent,
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    FormsModule,
    HttpClientModule,
    AppRoutingModule,
    CoreModule,
    SharedModule,
    BookingModule
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
