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
  NbRadioModule,
} from '@nebular/theme';
import {HttpClientModule} from '@angular/common/http';


@NgModule({
  declarations: [InputFieldComponent],
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
  ],
  exports: [
    InputFieldComponent,
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
