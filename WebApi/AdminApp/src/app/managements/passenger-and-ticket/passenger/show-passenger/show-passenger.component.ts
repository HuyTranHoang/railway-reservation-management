import {Component, EventEmitter, Input, Output} from '@angular/core';
import {NbDialogRef} from '@nebular/theme';
import {Router} from '@angular/router';

@Component({
  selector: 'ngx-show-passenger',
  templateUrl: './show-passenger.component.html',
  styleUrls: ['./show-passenger.component.scss'],
})
export class ShowPassengerComponent {
  @Input() id: number;
  @Input() fullName: string;
  @Input() cardId: string;
  @Input() age: number;
  @Input() gender: string;
  @Input() phone: string;
  @Input() email: string;
  @Input() createdAt: string;

  @Output() onShowDelete = new EventEmitter<{ id: number, name: string }>();

  constructor(protected ref: NbDialogRef<ShowPassengerComponent>,
              private router: Router) {
  }

  dismiss() {
    this.ref.close();
  }

  onEdit() {
    this.router.navigateByUrl(`/managements/passenger-and-ticket/passenger/${this.id}/edit`);
    this.dismiss();
  }

  onDelete() {
    this.onShowDelete.emit({id: this.id, name: this.fullName});
    this.dismiss();
  }

}
