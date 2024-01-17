import {Component, EventEmitter, Input, Output} from '@angular/core';
import {NbDialogRef} from '@nebular/theme';
import {Router} from '@angular/router';

@Component({
  selector: 'ngx-show-cancellation-rule',
  templateUrl: './show-cancellation-rule.component.html',
  styleUrls: ['./show-cancellation-rule.component.scss']
})
export class ShowCancellationRuleComponent {
  @Input() id: number;
  @Input() departureDateDifference: number;
  @Input() fee: number;
  @Input() status: string;
  @Input() createdAt: string;


  @Output() onShowDelete = new EventEmitter<{ id: number, departureDateDifference: number }>();

  constructor(protected ref: NbDialogRef<ShowCancellationRuleComponent>,
              private router: Router) {
  }

  dismiss() {
    this.ref.close();
  }

  onEdit() {
    this.router.navigateByUrl(`/managements/payment-and-cancellation/cancellation-rule/${this.id}/edit`);
    this.dismiss();
  }

  onDelete() {
    this.onShowDelete.emit({id: this.id, departureDateDifference: this.departureDateDifference});
    this.dismiss();
  }
}
