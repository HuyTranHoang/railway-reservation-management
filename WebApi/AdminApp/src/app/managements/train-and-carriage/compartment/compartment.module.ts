import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ListCompartmentComponent } from './list-compartment/list-compartment.component';
import { AddCompartmentComponent } from './add-compartment/add-compartment.component';
import { EditCompartmentComponent } from './edit-compartment/edit-compartment.component';
import { ShowCompartmentComponent } from './show-compartment/show-compartment.component';
import { ConfirmDeleteCompartmentComponent } from './confirm-delete-compartment/confirm-delete-compartment.component';
import { SharedModule } from '../../shared/shared.module';
import { CompartmentRoutingModule } from './compartment-routing.module';
import { NbAutocompleteModule, NbStepperModule } from '@nebular/theme';


@NgModule({
  declarations: [
    ListCompartmentComponent,
    AddCompartmentComponent,
    EditCompartmentComponent,
    ShowCompartmentComponent,
    ConfirmDeleteCompartmentComponent,
  ],
  imports: [
    CommonModule,
    SharedModule,
    CompartmentRoutingModule,
    NbStepperModule,
    NbAutocompleteModule,
  ],
})
export class CompartmentModule { }
