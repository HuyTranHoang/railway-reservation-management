import {Component, EventEmitter, Input, Output} from '@angular/core';
import {NbDialogRef} from '@nebular/theme';
import {Router} from '@angular/router';

@Component({
  selector: 'ngx-show-carriage-type',
  templateUrl: './show-carriage-type.component.html',
  styleUrls: ['./show-carriage-type.component.scss'],
})
export class ShowCarriageTypeComponent {
  @Input() id: number;
  @Input() name: string;
  @Input() numberOfCompartments: number;
  @Input() serviceCharge: number;
  @Input() description: string;
  @Input() status: string;
  @Input() createdAt: string;

  @Output() onShowDelete = new EventEmitter<{id: number, name: string}>();

  constructor(protected ref: NbDialogRef<ShowCarriageTypeComponent>,
              private router: Router) {}

  dismiss() {
    this.ref.close();
  }

  onEdit() {
    this.router.navigateByUrl(`/managements/train-and-carriage/carriage-type/${this.id}/edit`);
    this.dismiss();
  }

  onDelete() {
    this.onShowDelete.emit({id: this.id, name: this.name});
    this.dismiss();
  }
}
