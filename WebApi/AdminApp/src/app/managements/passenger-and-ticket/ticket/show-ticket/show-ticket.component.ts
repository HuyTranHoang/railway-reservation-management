import {Component, EventEmitter, Input, Output} from '@angular/core';
import {NbDialogRef} from '@nebular/theme';
import {Router} from '@angular/router';

@Component({
  selector: 'ngx-show-ticket',
  templateUrl: './show-ticket.component.html',
  styleUrls: ['./show-ticket.component.scss'],
})
export class ShowTicketComponent {
  @Input() id: number;
  @Input() code: string;
  @Input() passengerName: string;
  @Input() trainName: string;
  @Input() carriageName: string;
  @Input() seatName: string;
  @Input() scheduleName: string;
  @Input() price: number;
  @Input() status: string;
  @Input() createdAt: string;

  @Output() onShowCancel = new EventEmitter<{id: number, code: string}>();

  constructor(protected ref: NbDialogRef<ShowTicketComponent>,
              private router: Router) {}

  dismiss() {
    this.ref.close();
  }

  onCancel() {
    this.onShowCancel.emit({id: this.id, code: this.code});
    this.dismiss();
  }
}
