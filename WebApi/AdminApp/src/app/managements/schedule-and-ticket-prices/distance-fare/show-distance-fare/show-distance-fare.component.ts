import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {Router} from '@angular/router';
import {NbDialogRef} from '@nebular/theme';
import {DistanceFareService} from '../distance-fare.service';

@Component({
  selector: 'ngx-show-distance-fare',
  templateUrl: './show-distance-fare.component.html',
  styleUrls: ['./show-distance-fare.component.scss'],
})
export class ShowDistanceFareComponent implements OnInit {
  @Input() id: number;
  @Input() trainCompanyName: string;
  @Input() distance: number;
  @Input() price: number;
  @Input() status: string;
  @Input() createdAt: string;

  @Output() onShowDelete = new EventEmitter<{ id: number }>();

  constructor(private distanceFareService: DistanceFareService, protected ref: NbDialogRef<ShowDistanceFareComponent>,
              private router: Router) {
  }

  ngOnInit(): void {
  }


  dismiss() {
    this.ref.close();
  }

  onEdit() {
    this.router.navigateByUrl(`/managements/schedule-and-ticket-prices/distance-fare/edit/${this.id}`);
    this.dismiss();
  }

  onDelete() {
    this.onShowDelete.emit({id: this.id});
    this.dismiss();
  }
}
