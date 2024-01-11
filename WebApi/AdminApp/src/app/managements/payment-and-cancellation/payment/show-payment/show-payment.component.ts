import {Component, EventEmitter, Input, Output} from '@angular/core';
import {NbDialogRef} from '@nebular/theme';
import {Router} from '@angular/router';

@Component({
  selector: 'ngx-show-payment',
  templateUrl: './show-payment.component.html',
  styleUrls: ['./show-payment.component.scss'],
})
export class ShowPaymentComponent {
  @Input() id: number;
  @Input() fullName: string;
  @Input() email: string;
  @Input() status: string;
  @Input() createdAt: string;

  @Output() onShowDelete = new EventEmitter<{ id: number, name: string }>();

  constructor(protected ref: NbDialogRef<ShowPaymentComponent>,
              private router: Router) {
  }

  dismiss() {
    this.ref.close();
  }

  onEdit() {
    this.router.navigateByUrl(`/managements/payment-and-cancellation/payment/${this.id}/edit`);
    this.dismiss();
  }

  onDelete() {
    this.onShowDelete.emit({id: this.id, name: this.fullName});
    this.dismiss();
  }

}
