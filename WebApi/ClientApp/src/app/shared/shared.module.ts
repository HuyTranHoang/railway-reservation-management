import { NgModule } from '@angular/core';
import { NgxSpinnerModule } from 'ngx-spinner'
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker'
import { AccordionModule } from 'ngx-bootstrap/accordion';
import { TabsModule } from 'ngx-bootstrap/tabs'
import { TooltipModule } from 'ngx-bootstrap/tooltip'

@NgModule({
  declarations: [],
  imports: [
    NgxSpinnerModule,
    BsDatepickerModule.forRoot(),
    AccordionModule.forRoot(),
    TabsModule.forRoot(),
    TooltipModule.forRoot()

  ],
  exports: [
    NgxSpinnerModule,
    BsDatepickerModule,
    AccordionModule,
    TabsModule,
    TooltipModule
  ]
})
export class SharedModule { }
