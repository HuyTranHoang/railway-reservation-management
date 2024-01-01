import { NgxSpinnerModule }                    from 'ngx-spinner'
import { BrowserAnimationsModule }             from '@angular/platform-browser/animations';
import { BrowserModule }                       from '@angular/platform-browser';
import { NgModule }                            from '@angular/core';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http'
import { HomeComponent }                       from './home/home.component';
import { LoadingInterceptor }                  from './core/interceptors/loading.interceptor'
import { ErrorInterceptor }                    from './core/interceptors/error.interceptor'
import { CoreModule }                          from './core/core.module'
import { AppComponent }                        from './app.component';
import { AppRoutingModule }                    from './app-routing.module';
import { BookingTrainComponent } from './booking-train/booking-train.component';


@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    BookingTrainComponent,

  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    HttpClientModule,
    CoreModule,
    NgxSpinnerModule
  ],
  providers: [
    {provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true},
    {provide: HTTP_INTERCEPTORS, useClass: LoadingInterceptor, multi: true},
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
