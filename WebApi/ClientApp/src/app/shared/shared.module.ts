import { NgModule } from '@angular/core';
import { NgxSpinnerModule } from 'ngx-spinner'
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker'


@NgModule({
  declarations: [],
  imports: [
    NgxSpinnerModule,
    BsDatepickerModule.forRoot()
  ],
  exports: [
    NgxSpinnerModule,
    BsDatepickerModule
  ]
})
export class SharedModule { }
