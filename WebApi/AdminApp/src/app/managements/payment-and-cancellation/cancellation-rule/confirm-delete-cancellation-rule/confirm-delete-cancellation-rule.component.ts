import {Component, EventEmitter, Input, Output} from '@angular/core';
import {NbDialogRef, NbGlobalPhysicalPosition, NbToastrService} from '@nebular/theme';
import { CancellationRuleService } from '../cancellation-rule.service';

@Component({
  selector: 'ngx-confirm-delete-cancellation-rule',
  templateUrl: './confirm-delete-cancellation-rule.component.html',
  styleUrls: ['./confirm-delete-cancellation-rule.component.scss']
})
export class ConfirmDeleteCancellationRuleComponent {
  @Input() id: number;
  @Input() departureDateDifference: number;
  @Input() fee: number;

  @Output() onConfirmDelete = new EventEmitter<void>();

  constructor(protected ref: NbDialogRef<ConfirmDeleteCancellationRuleComponent>,
              private cancellationRuleService: CancellationRuleService,
              private toastrService: NbToastrService) {}

  dismiss() {
    this.ref.close();
  }

  onDelete() {
    this.cancellationRuleService.deleteCancellationRule(this.id).subscribe({
      next: _ => {
        this.showToast('success', 'Success', 'Delete cancellation rule successfully!');
        this.onConfirmDelete.emit();
        this.ref.close();
      },
      error: _ => {
        this.showToast('danger', 'Failed', 'Delete cancellation rule failed!');
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
