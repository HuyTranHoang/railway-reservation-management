import { Component, EventEmitter, Input, Output } from '@angular/core';
import { NbDialogRef, NbGlobalPhysicalPosition, NbToastrService } from '@nebular/theme';
import { DistanceFareService } from '../distance-fare.service';

@Component({
  selector: 'ngx-confirm-delete-distance-fare',
  templateUrl: './confirm-delete-distance-fare.component.html',
  styleUrls: ['./confirm-delete-distance-fare.component.scss']
})
export class ConfirmDeleteDistanceFareComponent {
  @Input() id: number;
  @Output() onConfirmDelete = new EventEmitter<void>();

  constructor(protected ref: NbDialogRef<ConfirmDeleteDistanceFareComponent>,
              private distanceFareService: DistanceFareService,
              private toastrService: NbToastrService) {
  }

  dismiss() {
    this.ref.close();
  }

  onDeleteSeatType() {
    this.distanceFareService.deleteDistanceFare(this.id).subscribe({
      next: res => {
        this.showToast('success', 'Success', 'Delete seat type successfully!');
        this.onConfirmDelete.emit();
        this.ref.close();
      },
      error: error => {
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
