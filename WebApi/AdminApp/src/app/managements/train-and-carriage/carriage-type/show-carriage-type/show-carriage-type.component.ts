import {Component, Input} from '@angular/core';
import {NbDialogRef} from '@nebular/theme';

@Component({
  selector: 'ngx-show-carriage-type',
  templateUrl: './show-carriage-type.component.html',
  styleUrls: ['./show-carriage-type.component.scss'],
})
export class ShowCarriageTypeComponent {
  @Input() name: string;
  @Input() serviceCharge: number;
  @Input() description: string;
  @Input() status: string;
  @Input() createdAt: string;

  constructor(protected ref: NbDialogRef<ShowCarriageTypeComponent>) {}

  dismiss() {
    this.ref.close();
  }

  onEdit() {

  }

  onDelete() {

  }
}
