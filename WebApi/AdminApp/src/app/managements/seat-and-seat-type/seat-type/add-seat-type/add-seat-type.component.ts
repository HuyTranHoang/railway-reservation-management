import {Component, OnInit} from '@angular/core';

import {AbstractControl, FormBuilder, FormGroup, ValidationErrors, ValidatorFn, Validators} from '@angular/forms';
import {NbComponentStatus, NbGlobalPhysicalPosition, NbToastrService} from '@nebular/theme';
import { SeatTypeService } from '../seat-type.service';

@Component({
  selector: 'ngx-add-seat-type',
  templateUrl: './add-seat-type.component.html',
  styleUrls: ['./add-seat-type.component.scss']
})
export class AddSeatTypeComponent {
  seatTypeForm: FormGroup = this.fb.group({});

  constructor(private seatTypeService: SeatTypeService,
              private toastrService: NbToastrService,
              private fb: FormBuilder) {
  }

  ngOnInit(): void {
    this.initForm();
  }

  initForm() {
    this.seatTypeForm = this.fb.group({
      name: ['', Validators.required],
      serviceCharge: [0, [Validators.required, Validators.min(0), this.numberValidator()]],
      status: ['', Validators.required],
      description: ['', Validators.required],
    });
  }

  numberValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      if (control.value == null || control.value === '') return null;
      return !isNaN(parseFloat(control.value)) && isFinite(control.value) ? null : { 'notANumber': true };
    };
  }

  onSubmit() {
    if (this.seatTypeForm.valid) {
      this.seatTypeService.addSeatType(this.seatTypeForm.value).subscribe({
        next: (res) => {
          this.showToast('success', 'Add carriage type successfully!');
          this.seatTypeForm.reset();
        },
        error: (err) => {
          this.showToast('danger', 'Add carriage type failed!');
        },
      });
    }
  }

  private showToast(type: string, body: string) {
    const config = {
      status: type,
      destroyByClick: true,
      duration: 2000,
      hasIcon: true,
      position: NbGlobalPhysicalPosition.TOP_RIGHT,
    };
    this.toastrService.show(
      body,
      'Success',
      config);
  }
}
