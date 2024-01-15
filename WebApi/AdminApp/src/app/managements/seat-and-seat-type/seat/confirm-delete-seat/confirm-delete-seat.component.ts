import { SeatService } from '../seat.service';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { NbDialogRef, NbToastrService, NbGlobalPhysicalPosition } from '@nebular/theme';
import { ConfirmDeleteTrainComponent } from '../../../train-and-carriage/train/confirm-delete-train/confirm-delete-train.component';

@Component({
  selector: 'ngx-confirm-delete-seat',
  templateUrl: './confirm-delete-seat.component.html',
  styleUrls: ['./confirm-delete-seat.component.scss']
})
export class ConfirmDeleteSeatComponent {
  @Input() id: number;
  @Input() name: string;
  @Output() ConfirmDelete = new EventEmitter<void>();

  constructor(protected ref: NbDialogRef<ConfirmDeleteTrainComponent>,
              private seatService: SeatService,
              private toastrService: NbToastrService) {
  }

  dismiss() {
    this.ref.close();
  }

  onDelete() {
    this.seatService.deleteSeat(this.id).subscribe({
      next: _ => {
        this.showToast('success', 'Success', 'Delete seat successfully!');
        this.ConfirmDelete.emit();
        this.ref.close();
      },
      error: _ => {
        this.showToast('danger', 'Failed', 'Delete seat failed!');
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
