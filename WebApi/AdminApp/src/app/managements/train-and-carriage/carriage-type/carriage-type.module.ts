import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {ListCarriageTypeComponent} from './list-carriage-type/list-carriage-type.component';
import {SharedModule} from '../../shared/shared.module';
import {AddCarriageTypeComponent} from './add-carriage-type/add-carriage-type.component';
import {EditCarriageTypeComponent} from './edit-carriage-type/edit-carriage-type.component';
import {
  ConfirmDeleteCarriageTypeComponent,
} from './confirm-delete-carriage-type/confirm-delete-carriage-type.component';
import {ShowCarriageTypeComponent} from './show-carriage-type/show-carriage-type.component';
import {CarriageTypeRoutingModule} from './carriage-type-routing.module';


@NgModule({
  declarations: [
    ListCarriageTypeComponent,
    AddCarriageTypeComponent,
    EditCarriageTypeComponent,
    ShowCarriageTypeComponent,
    ConfirmDeleteCarriageTypeComponent,
  ],
  imports: [
    CommonModule,
    SharedModule,
    CarriageTypeRoutingModule,
  ],
})
export class CarriageTypeModule {
}
