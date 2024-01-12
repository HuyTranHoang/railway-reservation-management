import { Component, EventEmitter, Input, Output } from '@angular/core';
import { NbDialogRef, NbGlobalPhysicalPosition, NbToastrService } from '@nebular/theme';
import { TrainStationService } from '../train-station.service';

@Component({
  selector: 'ngx-delete-train-station',
  templateUrl: './delete-train-station.component.html',
  styleUrls: ['./delete-train-station.component.scss']
})
export class DeleteTrainStationComponent {
  @Input() id: number;
  @Input() name: string;
  @Output() onConfirmDelete = new EventEmitter<void>();

  constructor(protected ref: NbDialogRef<DeleteTrainStationComponent>,
              private trainStationService: TrainStationService,
              private toastrService: NbToastrService) {}

  dismiss() {
    this.ref.close();
  }

  onDeleteSeatType() {
    console.log(this.id);
    this.trainStationService.deleteTrainStation(this.id).subscribe({
      next: res => {
        console.log(res);
        this.showToast('success', 'Success', 'Delete Train Station successfully!');
        this.onConfirmDelete.emit();
        this.ref.close();
      },
      error: error => {
        console.log(error);
        this.showToast('danger', 'Failed', 'Delete Train Station failed!');
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
