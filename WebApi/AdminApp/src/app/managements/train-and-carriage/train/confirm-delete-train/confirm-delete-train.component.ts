import {Component, EventEmitter, Input, Output} from '@angular/core';
import {NbDialogRef, NbToastrService, NbGlobalPhysicalPosition} from '@nebular/theme';
import {TrainService} from '../train.service';

@Component({
  selector: 'ngx-confirm-delete-train',
  templateUrl: './confirm-delete-train.component.html',
  styleUrls: ['./confirm-delete-train.component.scss'],
})
export class ConfirmDeleteTrainComponent {

  @Input() id: number;
  @Input() name: string;
  @Output() onConfirmDelete = new EventEmitter<void>();

  constructor(protected ref: NbDialogRef<ConfirmDeleteTrainComponent>,
              private trainService: TrainService,
              private toastrService: NbToastrService) {
  }

  dismiss() {
    this.ref.close();
  }

  onDelete() {
    this.trainService.deleteTrain(this.id).subscribe({
      next: _ => {
        this.showToast('success', 'Success', 'Delete train successfully!');
        this.onConfirmDelete.emit();
        this.ref.close();
      },
      error: _ => {
        this.showToast('danger', 'Failed', 'Delete train failed!');
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
