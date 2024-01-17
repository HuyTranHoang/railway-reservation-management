import {Component, EventEmitter, Input, Output} from '@angular/core';
import {NbDialogRef, NbGlobalPhysicalPosition, NbToastrService} from '@nebular/theme';
import {ScheduleService} from '../schedule.service';

@Component({
  selector: 'ngx-confirm-delete-schedule',
  templateUrl: './confirm-delete-schedule.component.html',
  styleUrls: ['./confirm-delete-schedule.component.scss'],
})
export class ConfirmDeleteScheduleComponent {
  @Input() id: number;
  @Input() name: string;
  @Output() onConfirmDelete = new EventEmitter<void>();

  constructor(protected ref: NbDialogRef<ConfirmDeleteScheduleComponent>,
              private scheduleService: ScheduleService,
              private toastrService: NbToastrService) {
  }

  dismiss() {
    this.ref.close();
  }

  onDelete() {
    this.scheduleService.deleteSchedule(this.id).subscribe({
      next: _ => {
        this.showToast('success', 'Success', 'Delete schedule successfully!');
        this.onConfirmDelete.emit();
        this.ref.close();
      },
      error: _ => {
        this.showToast('danger', 'Failed', 'Delete schedule failed!');
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
