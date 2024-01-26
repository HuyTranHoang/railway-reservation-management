import { NgModule } from '@angular/core';
import { NgxSpinnerModule } from 'ngx-spinner'
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker'
import { AccordionModule } from 'ngx-bootstrap/accordion';
import { TabsModule } from 'ngx-bootstrap/tabs'
import { TooltipModule } from 'ngx-bootstrap/tooltip'
import { AlertModule } from 'ngx-bootstrap/alert'

@NgModule({
  declarations: [],
  imports: [
    NgxSpinnerModule,
    BsDatepickerModule.forRoot(),
    AccordionModule.forRoot(),
    TabsModule.forRoot(),
    TooltipModule.forRoot(),
    AlertModule.forRoot()

  ],
  exports: [
    NgxSpinnerModule,
    BsDatepickerModule,
    AccordionModule,
    TabsModule,
    TooltipModule,
    AlertModule
  ]
})
export class SharedModule { }
