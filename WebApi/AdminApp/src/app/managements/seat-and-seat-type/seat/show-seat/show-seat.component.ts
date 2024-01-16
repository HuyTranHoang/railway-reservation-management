import {Component, EventEmitter, Input, Output} from '@angular/core';
import {Router} from '@angular/router';
import {NbDialogRef} from '@nebular/theme';

@Component({
  selector: 'ngx-show-seat',
  templateUrl: './show-seat.component.html',
  styleUrls: ['./show-seat.component.scss']
})
export class ShowSeatComponent {
  @Input() id: number;
  @Input() name: string;
  @Input() seatTypeName : string;
  @Input() compartmentName: string;
  @Input() status: string;
  @Input() createdAt: string;

  @Output() onShowDelete = new EventEmitter<{ id: number, name: string }>();

  constructor(protected ref: NbDialogRef<ShowSeatComponent>,
              private router: Router) {
  }

  dismiss() {
    this.ref.close();
  }

  onEdit() {
    this.router.navigateByUrl(`/managements/seat-and-seat-type/seat/${this.id}/edit`);
    this.dismiss();
  }

  onDelete() {
    this.onShowDelete.emit({id: this.id, name: this.name});
    this.dismiss();
  }
}
