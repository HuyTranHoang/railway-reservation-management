import {Component, EventEmitter, Input, Output} from '@angular/core';
import {NbDialogRef} from '@nebular/theme';
import {Router} from '@angular/router';

@Component({
  selector: 'ngx-show-carriage',
  templateUrl: './show-carriage.component.html',
  styleUrls: ['./show-carriage.component.scss'],
})
export class ShowCarriageComponent {
  @Input() id: number;
  @Input() name: string;
  @Input() trainName: string;
  @Input() carriageTypeName: string;
  @Input() numberOfCompartments: number;
  @Input() status: string;
  @Input() createdAt: string;

  @Output() onShowDelete = new EventEmitter<{ id: number, name: string }>();

  constructor(protected ref: NbDialogRef<ShowCarriageComponent>,
              private router: Router) {
  }

  dismiss() {
    this.ref.close();
  }

  onEdit() {
    this.router.navigateByUrl(`/managements/train-and-carriage/carriage/${this.id}/edit`);
    this.dismiss();
  }

  onDelete() {
    this.onShowDelete.emit({id: this.id, name: this.name});
    this.dismiss();
  }

}
