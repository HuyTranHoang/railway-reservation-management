import {Component, EventEmitter, Input, Output} from '@angular/core';
import {NbDialogRef, NbGlobalPhysicalPosition, NbToastrService} from '@nebular/theme';
import {CancellationService} from '../cancellation.service';


@Component({
  selector: 'ngx-confirm-delete-cancellation',
  templateUrl: './confirm-delete-cancellation.component.html',
  styleUrls: ['./confirm-delete-cancellation.component.scss']
})
export class ConfirmDeleteCancellationComponent {
  @Input() id: number;
  @Input() ticketCode: string;

  @Output() onConfirmDelete = new EventEmitter<void>();

  constructor(protected ref: NbDialogRef<ConfirmDeleteCancellationComponent>,
              private cancellationService: CancellationService,
              private toastrService: NbToastrService) {}

  dismiss() {
    this.ref.close();
  }

  onDelete() {
    this.cancellationService.deleteCancellation(this.id).subscribe({
      next: _ => {
        this.showToast('success', 'Success', 'Delete cancellation successfully!');
        this.onConfirmDelete.emit();
        this.ref.close();
      },
      error: _ => {
        this.showToast('danger', 'Failed', 'Delete cancellation failed!');
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
