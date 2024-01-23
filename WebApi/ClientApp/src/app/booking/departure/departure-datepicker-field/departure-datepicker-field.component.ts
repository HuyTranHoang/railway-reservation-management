import { Component, EventEmitter, Input, Output } from '@angular/core'
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker'

@Component({
  selector: 'app-departure-datepicker-field',
  templateUrl: './departure-datepicker-field.component.html',
  styleUrls: ['../departure.shared.scss']
})
export class DepartureDatepickerFieldComponent {
  @Input() label: string = '';
  @Input() placeholder: string = '';
  @Input() icon: string = '';
  @Input() date: Date | undefined
  @Input() isDisabled: boolean = false;
  @Output() onHandleDateChange = new EventEmitter<Date>();

  colorTheme = 'theme-orange';
  bsConfig?: Partial<BsDatepickerConfig> = Object.assign({}, { containerClass: this.colorTheme });

  onDateChange(newDate: any) {
    this.onHandleDateChange.emit(newDate);
  }

}
