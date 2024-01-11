import {Component, Input} from '@angular/core';
import {AbstractControl} from '@angular/forms';

@Component({
  selector: 'app-input-field',
  templateUrl: './input-field.component.html',
  styleUrls: ['./input-field.component.scss'],
})
export class InputFieldComponent {
  @Input() control: AbstractControl;
  @Input() label: string;
  @Input() placeholder: string;
  @Input() isSubmitted: boolean;
  @Input() type: string = 'text';

}
