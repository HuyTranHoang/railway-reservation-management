import {Component, EventEmitter, Input, Output} from '@angular/core';
import {NbDialogRef, NbGlobalPhysicalPosition, NbToastrService} from '@nebular/theme';
import {CarriageTypeService} from '../carriage-type.service';

@Component({
  selector: 'ngx-confirm-delete-carriage-type',
  templateUrl: './confirm-delete-carriage-type.component.html',
  styleUrls: ['./confirm-delete-carriage-type.component.scss'],
})
export class ConfirmDeleteCarriageTypeComponent {
  @Input() id: number;
  @Input() name: string;
  @Output() onConfirmDelete = new EventEmitter<void>();

  constructor(protected ref: NbDialogRef<ConfirmDeleteCarriageTypeComponent>,
              private carriageTypeService: CarriageTypeService,
              private toastrService: NbToastrService) {}

  dismiss() {
    this.ref.close();
  }

  onDelete() {
    this.carriageTypeService.deleteCarriageType(this.id).subscribe({
      next: _ => {
        this.showToast('success', 'Success', 'Delete carriage type successfully!');
        this.onConfirmDelete.emit();
        this.ref.close();
      },
      error: _ => {
        this.showToast('danger', 'Failed', 'Delete carriage type failed!');
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
