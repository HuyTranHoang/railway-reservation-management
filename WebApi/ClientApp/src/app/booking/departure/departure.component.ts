import { Component } from '@angular/core';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker'

@Component({
  selector: 'app-departure',
  templateUrl: './departure.component.html',
  styleUrls: ['./departure.component.scss', './departure.shared.scss']
})
export class DepartureComponent {


  isModify = false;

  onModifyButtonClicked(isModified: boolean) {
    this.isModify = isModified;
  }

}
