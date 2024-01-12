import {Component, EventEmitter, Input, Output} from '@angular/core';
import {NbDialogRef, NbGlobalPhysicalPosition, NbToastrService} from '@nebular/theme';
import {RoundTripService} from '../round-trip.service';

@Component({
  selector: 'ngx-confirm-delete-round-trip',
  templateUrl: './confirm-delete-round-trip.component.html',
  styleUrls: ['./confirm-delete-round-trip.component.scss'],
})
export class ConfirmDeleteRoundTripComponent {
  @Input() id: number;
  @Input() name: string;

  @Output() onConfirmDelete = new EventEmitter<void>();

  constructor(protected ref: NbDialogRef<ConfirmDeleteRoundTripComponent>,
              private roundTripService: RoundTripService,
              private toastrService: NbToastrService) {
  }

  dismiss() {
    this.ref.close();
  }

  onDelete() {
    this.roundTripService.deleteRoundTrip(this.id).subscribe({
      next: _ => {
        this.showToast('success', 'Success', 'Delete round trip successfully!');
        this.onConfirmDelete.emit();
        this.ref.close();
      },
      error: _ => {
        this.showToast('danger', 'Failed', 'Delete round trip failed!');
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
