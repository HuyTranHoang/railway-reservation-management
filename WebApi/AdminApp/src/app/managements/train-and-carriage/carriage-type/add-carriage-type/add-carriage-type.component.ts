import {Component, OnInit} from '@angular/core';
import {CarriageTypeService} from '../carriage-type.service';
import {AbstractControl, FormBuilder, FormGroup, ValidationErrors, ValidatorFn, Validators} from '@angular/forms';
import {NbGlobalPhysicalPosition, NbToastrService} from '@nebular/theme';


@Component({
  selector: 'ngx-add-carriage-type',
  templateUrl: './add-carriage-type.component.html',
  styleUrls: ['./add-carriage-type.component.scss'],
})
export class AddCarriageTypeComponent implements OnInit {
  carriageTypeForm: FormGroup = this.fb.group({});
  isSubmitted = false;
  errorMessages: string[] = [];

  constructor(private carriageTypeService: CarriageTypeService,
              private toastrService: NbToastrService,
              private fb: FormBuilder) {
  }

  ngOnInit(): void {
    this.initForm();
  }

  initForm() {
    this.carriageTypeForm = this.fb.group({
      name: ['', Validators.required],
      numberOfCompartments: [0, [Validators.required, Validators.min(1), this.numberValidator()]],
      serviceCharge: [0, [Validators.required, Validators.min(0), this.numberValidator()]],
      status: [''],
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
    this.isSubmitted = true;
    if (this.carriageTypeForm.valid) {
      this.carriageTypeService.addCarriageType(this.carriageTypeForm.value).subscribe({
        next: _ => {
          this.showToast('success', 'Success', 'Add carriage type successfully!');
          this.carriageTypeForm.reset();
          this.isSubmitted = false;
          this.errorMessages = [];
        },
        error: (err) => {
          this.showToast('danger', 'Failed', 'Add carriage type failed!');
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
