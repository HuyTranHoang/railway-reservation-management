import {Component, EventEmitter, Input, Output} from '@angular/core';
import {Router} from '@angular/router';
import {NbDialogRef} from '@nebular/theme';
import {DistanceFareService} from '../distance-fare.service';

@Component({
  selector: 'ngx-show-distance-fare',
  templateUrl: './show-distance-fare.component.html',
  styleUrls: ['./show-distance-fare.component.scss'],
})
export class ShowDistanceFareComponent {
  @Input() id: number;
  @Input() trainCompanyName: string;
  @Input() distance: number;
  @Input() price: number;
  @Input() status: string;
  @Input() createdAt: string;

  @Output() onShowDelete = new EventEmitter<{ id: number, trainCompanyName: string }>();

  constructor(private distanceFareService: DistanceFareService, protected ref: NbDialogRef<ShowDistanceFareComponent>,
              private router: Router) {
  }
  dismiss() {
    this.ref.close();
  }

  onEdit() {
    this.router.navigateByUrl(`/managements/schedule-and-ticket-prices/distance-fare/${this.id}/edit`);
    this.dismiss();
  }

  onDelete() {
    this.onShowDelete.emit({id: this.id, trainCompanyName: this.trainCompanyName});
    this.dismiss();
  }
}
