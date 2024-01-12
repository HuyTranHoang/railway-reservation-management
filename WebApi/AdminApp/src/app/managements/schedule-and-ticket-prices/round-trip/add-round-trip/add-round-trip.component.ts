import {Component, OnInit} from '@angular/core';
import {RoundTripService} from '../round-trip.service';
import {AbstractControl, FormBuilder, FormGroup, ValidationErrors, ValidatorFn, Validators} from '@angular/forms';
import {NbToastrService, NbGlobalPhysicalPosition} from '@nebular/theme';
import {RoundTrip} from '../../../../@models/roundTrip';
import {TrainCompany} from '../../../../@models/trainCompany';
import {TrainCompanyService} from '../../../railway/train-company/train-company.service';

@Component({
  selector: 'ngx-add-round-trip',
  templateUrl: './add-round-trip.component.html',
  styleUrls: ['./add-round-trip.component.scss'],
})
export class AddRoundTripComponent implements OnInit {
  trainCompanies: TrainCompany[] = [];

  roundTripForm: FormGroup = this.fb.group({});

  isSubmitted = false;
  errorMessages: string[] = [];

  constructor(
    private roundTripService: RoundTripService,
    private trainCompanyService: TrainCompanyService,
    private fb: FormBuilder,
    private toastrService: NbToastrService,
  ) {
  }

  ngOnInit(): void {
    this.initForm();
    this.loadAllTrainCompany();
  }

  initForm() {
    this.roundTripForm = this.fb.group({
      trainCompanyId: ['', Validators.required],
      discount: [0, [Validators.required, this.numberValidator()]],
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
    if (this.roundTripForm.valid) {
      this.roundTripService.addRoundTrip(this.roundTripForm.value).subscribe({
        next: (res) => {
          this.showToast('success', 'Success', 'Add round trip successfully!');
          this.roundTripForm.reset();
          this.isSubmitted = false;
          this.errorMessages = [];
        },
        error: (err) => {
          this.showToast('danger', 'Failed', 'Add round trip failed!');
          this.errorMessages = err.error.errors;
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

  loadAllTrainCompany() {
    this.trainCompanyService.getAllTrainCompanyNoPaging().subscribe({
      next: (res: TrainCompany[]) => {
        this.trainCompanies = res;
      },
    });
  }
}
