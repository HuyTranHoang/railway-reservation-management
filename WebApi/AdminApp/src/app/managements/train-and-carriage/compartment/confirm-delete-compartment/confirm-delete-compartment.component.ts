import {Component, EventEmitter, Input, Output} from '@angular/core';
import {NbDialogRef, NbGlobalPhysicalPosition, NbToastrService} from '@nebular/theme';
import {CompartmentService} from '../compartment.service';

@Component({
  selector: 'ngx-confirm-delete-compartment',
  templateUrl: './confirm-delete-compartment.component.html',
  styleUrls: ['./confirm-delete-compartment.component.scss']
})
export class ConfirmDeleteCompartmentComponent {
  @Input() id: number;
  @Input() name: string;

  @Output() onConfirmDelete = new EventEmitter<void>();

  constructor(protected ref: NbDialogRef<ConfirmDeleteCompartmentComponent>,
              private compartmentService: CompartmentService,
              private toastrService: NbToastrService) {
  }

  dismiss() {
    this.ref.close();
  }

  onDelete() {
    this.compartmentService.deleteCompartment(this.id).subscribe({
      next: _ => {
        this.showToast('success', 'Success', 'Delete compartment successfully!');
        this.onConfirmDelete.emit();
        this.ref.close();
      },
      error: _ => {
        this.showToast('danger', 'Failed', 'Delete compartment failed!');
      },
    });
  }

  private showToast(type: string, title: string, body: string) {
    const config = {
      status: type,
      destroyByClick: true,
      duration: 2000,
      hasIcon: true,
      position: NbGlobalPhysicalPosition.TOP_RIGHT,
    };
    this.toastrService.show(
      body,
      title,
      config);
  }
}
