import {Component, OnInit} from '@angular/core';

import {AbstractControl, FormBuilder, FormGroup, ValidationErrors, ValidatorFn, Validators} from '@angular/forms';
import {NbGlobalPhysicalPosition, NbToastrService} from '@nebular/theme';
import {SeatTypeService} from '../seat-type.service';

@Component({
  selector: 'ngx-add-seat-type',
  templateUrl: './add-seat-type.component.html',
  styleUrls: ['./add-seat-type.component.scss'],
})
export class AddSeatTypeComponent implements OnInit {
  seatTypeForm: FormGroup = this.fb.group({});

  isSubmitted = false;
  errorMessages: string[] = [];

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
      status: [''],
      description: ['', Validators.required],
    });
  }

  numberValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      if (control.value == null || control.value === '') return null;
      return !isNaN(parseFloat(control.value)) && isFinite(control.value) ? null : {'notANumber': true};
    };
  }

  onSubmit() {
    this.isSubmitted = true;

    if (this.seatTypeForm.valid) {
      this.seatTypeService.addSeatType(this.seatTypeForm.value).subscribe({
        next: (res) => {
          this.showToast('success', 'Success', 'Add seat type successfully!');
          this.seatTypeForm.reset();
          this.isSubmitted = false;
          this.errorMessages = [];
        },
        error: (err) => {
          this.showToast('danger', 'Failed', 'Add seat type failed!');
          this.errorMessages = err.error.errorMessages;
        },
      });
    }
  }

  private showToast(type: string, title: string, body: string) {
    const config = {
      status: type,
      destroyByClick: true,
      duration: 2000,
      hasIcon: true,
      position: NbGlobalPhysicalPosition.TOP_RIGHT,
    };
    this.toastrService.show(
      body,
      title,
      config);
  }
}
