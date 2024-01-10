import {Component, EventEmitter, Input, Output} from '@angular/core';
import {NbDialogRef, NbGlobalPhysicalPosition, NbToastrService} from '@nebular/theme';
import {SeatTypeService} from '../../../seat-and-seat-type/seat-type/seat-type.service';
import {PassengerService} from '../passenger.service';

@Component({
  selector: 'ngx-confirm-delete-passenger',
  templateUrl: './confirm-delete-passenger.component.html',
  styleUrls: ['./confirm-delete-passenger.component.scss'],
})
export class ConfirmDeletePassengerComponent {
  @Input() id: number;
  @Input() name: string;
  @Output() onConfirmDelete = new EventEmitter<void>();

  constructor(protected ref: NbDialogRef<ConfirmDeletePassengerComponent>,
              private passengerService: PassengerService,
              private toastrService: NbToastrService) {
  }

  dismiss() {
    this.ref.close();
  }

  onDeletePassenger() {
    this.passengerService.deletePassenger(this.id).subscribe({
      next: res => {
        this.showToast('success', 'Success', 'Delete passenger successfully!');
        this.onConfirmDelete.emit();
        this.ref.close();
      },
      error: error => {
        this.showToast('danger', 'Failed', 'Delete passenger failed!');
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
