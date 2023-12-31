import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {InputFieldComponent} from './input-field/input-field.component';
import {ReactiveFormsModule} from '@angular/forms';
import {
  NbButtonModule,
  NbCardModule,
  NbCheckboxModule,
  NbIconModule,
  NbInputModule,
  NbRadioModule, NbSelectModule,
} from '@nebular/theme';
import {HttpClientModule} from '@angular/common/http';
import { PaginationComponent } from './pagination/pagination.component';


@NgModule({
  declarations: [InputFieldComponent, PaginationComponent],
  imports: [
    CommonModule,
    HttpClientModule,
    ReactiveFormsModule,
    NbButtonModule,
    NbCardModule,
    NbCheckboxModule,
    NbInputModule,
    NbIconModule,
    NbRadioModule,
    NbSelectModule,
  ],
  exports: [
    InputFieldComponent,
    PaginationComponent,
    ReactiveFormsModule,
    NbButtonModule,
    NbCardModule,
    NbCheckboxModule,
    NbInputModule,
    NbIconModule,
    NbRadioModule,
  ],
})
export class SharedModule {
}
