import { Component, Input } from '@angular/core'

@Component({
  selector: 'app-departure-input-field',
  templateUrl: './departure-input-field.component.html',
  styleUrls: ['../departure.shared.scss']
})
export class DepartureInputFieldComponent {
  @Input() label: string = '';
  @Input() placeholder: string = '';
  @Input() icon: string = '';
}
