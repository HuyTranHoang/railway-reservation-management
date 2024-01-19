import { NgModule } from '@angular/core';
import { NgxSpinnerModule } from 'ngx-spinner'
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker'
import { AccordionModule } from 'ngx-bootstrap/accordion';
import { CarouselModule } from 'ngx-bootstrap/carousel'

@NgModule({
  declarations: [],
  imports: [
    NgxSpinnerModule,
    BsDatepickerModule.forRoot(),
    AccordionModule.forRoot(),
    CarouselModule.forRoot()
  ],
  exports: [
    NgxSpinnerModule,
    BsDatepickerModule,
    AccordionModule,
    CarouselModule
  ]
})
export class SharedModule { }
