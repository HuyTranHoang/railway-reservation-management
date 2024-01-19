import { NgModule } from '@angular/core';
import { NgxSpinnerModule } from 'ngx-spinner'
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker'

import { AccordionModule } from 'ngx-bootstrap/accordion';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { NgImageSliderModule } from 'ng-image-slider';
import { CarouselModule } from 'ngx-bootstrap/carousel';


@NgModule({
  declarations: [],
  imports: [
    NgxSpinnerModule,
    BsDatepickerModule.forRoot(),
<<<<<<< HEAD
    CarouselModule
=======
    AccordionModule.forRoot(),
>>>>>>> a723e7c670a6d973797ed0e1ee5a091ad53409d3
  ],
  exports: [
    NgxSpinnerModule,
    BsDatepickerModule,
    AccordionModule
  ]
})
export class SharedModule { }
