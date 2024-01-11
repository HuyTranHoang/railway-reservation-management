import {Component, OnInit} from '@angular/core';
import {AbstractControl, FormBuilder, FormGroup, ValidationErrors, ValidatorFn, Validators} from '@angular/forms';
import {CarriageTypeService} from '../../../train-and-carriage/carriage-type/carriage-type.service';
import {NbGlobalPhysicalPosition, NbToastrService} from '@nebular/theme';
import {PassengerService} from '../passenger.service';

@Component({
  selector: 'ngx-add-passenger',
  templateUrl: './add-passenger.component.html',
  styleUrls: ['./add-passenger.component.scss'],
})
export class AddPassengerComponent implements OnInit {
  passengerForm: FormGroup = this.fb.group({});
  isSubmitted: boolean = false;
  errorMessages: string[] = [];

  constructor(private passengerSerivce: PassengerService,
              private toastrService: NbToastrService,
              private fb: FormBuilder) {
  }

  ngOnInit(): void {
    this.initForm();
  }

  initForm() {
    this.passengerForm = this.fb.group({
      fullName: ['', Validators.required],
      cardId: ['', Validators.required],
      age: ['', this.numberValidator()],
      gender: ['male', Validators.required],
      phone: ['', [Validators.required]], // Có thể cần validate lại phone sau
      email: ['', [Validators.required, Validators.email]],
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

    if (this.passengerForm.valid) {
      this.passengerSerivce.addPassenger(this.passengerForm.value).subscribe({
        next: (res) => {
          this.showToast('success', 'Success', 'Add passenger successfully!');
          this.passengerForm.reset();
          this.isSubmitted = false;
          this.errorMessages = [];
        },
        error: (err) => {
          this.errorMessages = err.error.errors;
          this.showToast('danger', 'Failed', 'Add passenger failed!');
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

  onGenderChange(event: string) {
    this.passengerForm.patchValue({
      gender: event,
    });
  }
}
