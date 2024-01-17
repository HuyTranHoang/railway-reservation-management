import {Component, EventEmitter, Input, Output} from '@angular/core';
import {NbDialogRef} from '@nebular/theme';
import {Router} from '@angular/router';

@Component({
  selector: 'ngx-show-round-trip',
  templateUrl: './show-round-trip.component.html',
  styleUrls: ['./show-round-trip.component.scss'],
})
export class ShowRoundTripComponent {
  @Input() id: number;
  @Input() trainCompanyName: string;
  @Input() discount: number;
  @Input() createdAt: string;


  @Output() onShowDelete = new EventEmitter<{ id: number, name: string }>();

  constructor(protected ref: NbDialogRef<ShowRoundTripComponent>,
              private router: Router) {
  }

  dismiss() {
    this.ref.close();
  }

  onEdit() {
    this.router.navigateByUrl(`/managements/schedule-and-ticket-prices/round-trip/${this.id}/edit`);
    this.dismiss();
  }

  onDelete() {
    this.onShowDelete.emit({id: this.id, name: this.trainCompanyName});
    this.dismiss();
  }
}
