import {Component, EventEmitter, Input, Output} from '@angular/core';
import {NbDialogRef, NbToastrService} from '@nebular/theme';
import {Router} from '@angular/router';

@Component({
  selector: 'ngx-show-train-company',
  templateUrl: './show-train-company.component.html',
  styleUrls: ['./show-train-company.component.scss'],
})
export class ShowTrainCompanyComponent {
  @Input() id: number;
  @Input() name: string;

  @Output() onShowDelete = new EventEmitter<{ id: number, name: string }>();

  constructor(protected ref: NbDialogRef<ShowTrainCompanyComponent>,
              private router: Router) {
  }

  dismiss() {
    this.ref.close();
  }

  onEdit() {
    this.router.navigateByUrl(`/managements/railway/train-company/${this.id}/edit`);
    this.dismiss();
  }

  onDelete() {
    this.onShowDelete.emit({id: this.id, name: this.name});
    this.dismiss();
  }
}
