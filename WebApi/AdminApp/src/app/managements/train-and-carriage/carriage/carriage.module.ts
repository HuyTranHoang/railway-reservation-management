import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {ListCarriageComponent} from './list-carriage/list-carriage.component';
import {AddCarriageComponent} from './add-carriage/add-carriage.component';
import {EditCarriageComponent} from './edit-carriage/edit-carriage.component';
import {ShowCarriageComponent} from './show-carriage/show-carriage.component';
import {ConfirmDeleteCarriageComponent} from './confirm-delete-carriage/confirm-delete-carriage.component';
import {SharedModule} from '../../shared/shared.module';
import {CarriageRoutingModule} from './carriage-routing.module';
import {NbAutocompleteModule, NbStepperModule} from '@nebular/theme';
import { PreviewCarriageComponent } from './preview-carriage/preview-carriage.component';


@NgModule({
  declarations: [
    ListCarriageComponent,
    AddCarriageComponent,
    EditCarriageComponent,
    ShowCarriageComponent,
    ConfirmDeleteCarriageComponent,
    PreviewCarriageComponent,
  ],
  imports: [
    CommonModule,
    SharedModule,
    CarriageRoutingModule,
    NbStepperModule,
    NbAutocompleteModule,
  ],
})
export class CarriageModule {
}
