import { Component } from '@angular/core';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker'

@Component({
  selector: 'app-departure',
  templateUrl: './departure.component.html',
  styleUrls: ['./departure.component.scss']
})
export class DepartureComponent {

  colorTheme = 'theme-orange';

  bsConfig?: Partial<BsDatepickerConfig> = Object.assign({}, { containerClass: this.colorTheme });

}
