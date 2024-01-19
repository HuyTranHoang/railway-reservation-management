import { NgModule } from '@angular/core';
import { NgxSpinnerModule } from 'ngx-spinner'
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker'

import { AccordionModule } from 'ngx-bootstrap/accordion';


@NgModule({
  declarations: [],
  imports: [
    NgxSpinnerModule,
    BsDatepickerModule.forRoot(),
    AccordionModule.forRoot(),
  ],
  exports: [
    NgxSpinnerModule,
    BsDatepickerModule,
    AccordionModule
  ]
})
export class SharedModule { }
