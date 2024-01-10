import {Component, EventEmitter, Input, Output} from '@angular/core';
import {NbDialogRef, NbGlobalPhysicalPosition, NbToastrService} from '@nebular/theme';
import { TrainCompanyService } from '../train-company.service';

@Component({
  selector: 'ngx-confirm-delete-train-company',
  templateUrl: './confirm-delete-train-company.component.html',
  styleUrls: ['./confirm-delete-train-company.component.scss']
})
export class ConfirmDeleteTrainCompanyComponent {
  @Input() id: number;
  @Input() name: string;
  @Output() onConfirmDelete = new EventEmitter<void>();

  constructor(protected ref: NbDialogRef<ConfirmDeleteTrainCompanyComponent>,
              private trainCompanyService: TrainCompanyService,
              private toastrService: NbToastrService) {}

  dismiss() {
    this.ref.close();
  }

  onDelete() {
    this.trainCompanyService.deleteTrainCompany(this.id).subscribe({
      next: _ => {
        this.showToast('success', 'Success', 'Delete train company successfully!');
        this.onConfirmDelete.emit();
        this.ref.close();
      },
      error: _ => {
        this.showToast('danger', 'Failed', 'Delete train company failed!');
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
