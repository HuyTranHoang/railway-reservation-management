import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Router } from '@angular/router';
import { NbDialogRef } from '@nebular/theme';

@Component({
  selector: 'ngx-show-train-station',
  templateUrl: './show-train-station.component.html',
  styleUrls: ['./show-train-station.component.scss']
})
export class ShowTrainStationComponent {
  @Input() id: number;
  @Input() name: string;
  @Input() address: string;
  @Input() coordinateValue: number;
  @Input() status: string;

  @Output() onShowDelete = new EventEmitter<{id: number, name: string}>();

  constructor(protected ref: NbDialogRef<ShowTrainStationComponent>,
              private router: Router) {}

  dismiss() {
    this.ref.close();
  }

  onEdit() {
    this.router.navigateByUrl(`/managements/railway/train-station/edit/${this.id}`);
    this.dismiss();
  }

  onDelete() {
    this.onShowDelete.emit({id: this.id, name: this.name});
    this.dismiss();
  }
}
