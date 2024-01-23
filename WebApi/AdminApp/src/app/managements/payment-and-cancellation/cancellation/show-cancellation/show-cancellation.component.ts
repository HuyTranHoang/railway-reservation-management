import {Component, EventEmitter, Input, Output} from '@angular/core';
import {NbDialogRef} from '@nebular/theme';
import {Router} from '@angular/router';

@Component({
  selector: 'ngx-show-cancellation',
  templateUrl: './show-cancellation.component.html',
  styleUrls: ['./show-cancellation.component.scss']
})
export class ShowCancellationComponent {
  @Input() id: number;
  @Input() ticketCode: string;
  @Input() departureDateDifference: number;
  @Input() reason: string;
  @Input() status: string;
  @Input() createdAt: string;


  @Output() onShowDelete = new EventEmitter<{ id: number, ticketCode: string }>();

  constructor(protected ref: NbDialogRef<ShowCancellationComponent>,
              private router: Router) {
  }

  dismiss() {
    this.ref.close();
  }

  onEdit() {
    this.router.navigateByUrl(`/managements/payment-and-cancellation/cancellation/${this.id}/edit`);
    this.dismiss();
  }

  onDelete() {
    this.onShowDelete.emit({id: this.id, ticketCode: this.ticketCode});
    this.dismiss();
  }
}
