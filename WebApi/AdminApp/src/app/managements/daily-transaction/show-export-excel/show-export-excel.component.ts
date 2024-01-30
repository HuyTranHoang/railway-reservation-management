import {Component, EventEmitter, Input, Output} from '@angular/core';
import {NbDialogRef} from '@nebular/theme';
import {Router} from '@angular/router';
import {DailyCashTransaction} from '../../../@models/dailyCashTransaction';

@Component({
  selector: 'ngx-show-export-excel',
  templateUrl: './show-export-excel.component.html',
  styleUrls: ['./show-export-excel.component.scss'],
})
export class ShowExportExcelComponent {
  @Input() data: DailyCashTransaction[];

  startDateString: string = '';
  endDateString: string = '';

  @Output() onShowExport = new EventEmitter<{ startDate: Date, endDate: Date }>();

  constructor(protected ref: NbDialogRef<ShowExportExcelComponent>,
              private router: Router) {
  }

  dismiss() {
    this.ref.close();
  }

  onExport() {

    const startDate = new Date(this.startDateString);
    const endDate = new Date(this.endDateString);

    this.onShowExport.emit({startDate, endDate});
    this.dismiss();
  }
}
