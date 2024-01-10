import {Component, OnInit} from '@angular/core';
import {AbstractControl, FormBuilder, FormGroup, ValidationErrors, ValidatorFn, Validators} from '@angular/forms';
import {PassengerService} from '../passenger.service';
import {NbGlobalPhysicalPosition, NbToastrService} from '@nebular/theme';
import {ActivatedRoute, Router} from '@angular/router';

@Component({
  selector: 'ngx-edit-passenger',
  templateUrl: './edit-passenger.component.html',
  styleUrls: ['./edit-passenger.component.scss'],
})
export class EditPassengerComponent implements OnInit {
  passengerForm: FormGroup = this.fb.group({});
  isSubmitted: boolean = false;
  errorMessages: string[] = [];

  constructor(private passengerSerivce: PassengerService,
              private toastrService: NbToastrService,
              private fb: FormBuilder,
              private activatedRoute: ActivatedRoute,
              private router: Router) {
  }

  ngOnInit(): void {
    this.initForm();
  }

  initForm() {
    this.passengerForm = this.fb.group({
      id: ['', Validators.required],
      fullName: ['', Validators.required],
      cardId: ['', Validators.required],
      age: ['', this.numberValidator()],
      gender: ['male', Validators.required],
      phone: ['', [Validators.required]], // Có thể cần validate lại phone sau
      email: ['', [Validators.required, Validators.email]],
    });

    const id = this.activatedRoute.snapshot.params.id;

    this.passengerSerivce.getPassengerById(id).subscribe({
      next: (res) => {
        this.passengerForm.patchValue(res);
      },
      error: (err) => {
        this.showToast('danger', 'Failed', 'Passenger doest not exist!');
        this.router.navigateByUrl('/managements/passenger-and-ticket/passenger');
      },
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

    if (this.passengerForm.valid) {
      this.passengerSerivce.updatePassenger(this.passengerForm.value).subscribe({
        next: (res) => {
          this.showToast('success', 'Success', 'Update passenger successfully!');
          this.isSubmitted = false;
          this.errorMessages = [];
        },
        error: (err) => {
          this.errorMessages = err.error.errors;
          this.showToast('danger', 'Failed', 'Update passenger failed!');
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
