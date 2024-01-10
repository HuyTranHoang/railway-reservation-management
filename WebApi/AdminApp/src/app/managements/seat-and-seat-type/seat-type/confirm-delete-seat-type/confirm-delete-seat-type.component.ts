import {Component, EventEmitter, Input, Output} from '@angular/core';
import {NbDialogRef, NbGlobalPhysicalPosition, NbToastrService} from '@nebular/theme';
import { SeatTypeService } from '../seat-type.service';


@Component({
  selector: 'ngx-confirm-delete-seat-type',
  templateUrl: './confirm-delete-seat-type.component.html',
  styleUrls: ['./confirm-delete-seat-type.component.scss']
})
export class ConfirmDeleteSeatTypeComponent {
  @Input() id: number;
  @Input() name: string;
  @Output() onConfirmDelete = new EventEmitter<void>();

  constructor(protected ref: NbDialogRef<ConfirmDeleteSeatTypeComponent>,
              private seatTypeService: SeatTypeService,
              private toastrService: NbToastrService) {}

  dismiss() {
    this.ref.close();
  }

  onDeleteSeatType() {
    console.log(this.id);
    this.seatTypeService.deleteSeatType(this.id).subscribe({
      next: res => {
        console.log(res);
        this.showToast('success', 'Success', 'Delete seat type successfully!');
        this.onConfirmDelete.emit();
        this.ref.close();
      },
      error: error => {
        console.log(error);
        this.showToast('danger', 'Failed', 'Delete seat type failed!');
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
