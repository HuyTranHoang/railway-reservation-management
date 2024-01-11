import {Component, EventEmitter, Input, Output} from '@angular/core';
import {Router} from '@angular/router';
import {NbDialogRef} from '@nebular/theme';

@Component({
  selector: 'ngx-show-seat-type',
  templateUrl: './show-seat-type.component.html',
  styleUrls: ['./show-seat-type.component.scss'],
})
export class ShowSeatTypeComponent {
  @Input() id: number;
  @Input() name: string;
  @Input() serviceCharge: number;
  @Input() description: string;
  @Input() status: string;
  @Input() createdAt: string;

  @Output() onShowDelete = new EventEmitter<{ id: number, name: string }>();

  constructor(protected ref: NbDialogRef<ShowSeatTypeComponent>,
              private router: Router) {
  }

  dismiss() {
    this.ref.close();
  }

  onEdit() {
    this.router.navigateByUrl(`/managements/seat-and-seat-type/seat-type/edit/${this.id}`);
    this.dismiss();
  }

  onDelete() {
    this.onShowDelete.emit({id: this.id, name: this.name});
    this.dismiss();
  }
}
