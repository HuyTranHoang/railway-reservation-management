import {Component, EventEmitter, Input, Output} from '@angular/core';
import {Router} from '@angular/router';
import {NbDialogRef} from '@nebular/theme';

@Component({
  selector: 'ngx-show-schedule',
  templateUrl: './show-schedule.component.html',
  styleUrls: ['./show-schedule.component.scss'],
})

export class ShowScheduleComponent {

  @Input() id: number;
  @Input() name: string;
  @Input() trainName: string;
  @Input() departureStationName: string;
  @Input() arrivalStationName: string;
  @Input() arrivalTime: string;
  @Input() departureTime: string;
  @Input() duration: number;
  @Input() status: string;

  @Output() onShowDelete = new EventEmitter<{ id: number, name: string }>();

  constructor(protected ref: NbDialogRef<ShowScheduleComponent>,
              private router: Router) {
  }

  dismiss() {
    this.ref.close();
  }

  onEdit() {
    this.router.navigateByUrl(`/managements/schedule-and-ticket-prices/schedule/${this.id}/edit`);
    this.dismiss();
  }

  onDelete() {
    this.onShowDelete.emit({id: this.id, name: this.name});
    this.dismiss();
  }
}
