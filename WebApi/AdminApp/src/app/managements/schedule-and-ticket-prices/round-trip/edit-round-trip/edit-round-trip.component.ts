import {Component, OnInit} from '@angular/core';
import {AbstractControl, FormBuilder, FormGroup, ValidationErrors, ValidatorFn, Validators} from '@angular/forms';
import {RoundTripService} from '../round-trip.service';
import {NbGlobalPhysicalPosition, NbToastrService} from '@nebular/theme';
import {ActivatedRoute, Router} from '@angular/router';
import {TrainCompanyService} from '../../../railway/train-company/train-company.service';

import {TrainCompany} from '../../../../@models/trainCompany';


@Component({
  selector: 'ngx-edit-round-trip',
  templateUrl: './edit-round-trip.component.html',
  styleUrls: ['./edit-round-trip.component.scss'],
})
export class EditRoundTripComponent implements OnInit {
  trainCompanies: TrainCompany[] = [];
  updateForm: FormGroup = this.fb.group({});

  isSubmitted: boolean = false;
  errorMessages: string[] = [];

  constructor(private roundTripService: RoundTripService,
              private trainCompanyService: TrainCompanyService,
              private toastrService: NbToastrService,
              private activatedRoute: ActivatedRoute,
              private router: Router,
              private fb: FormBuilder) {
  }


  ngOnInit(): void {
    this.initForm();
    this.loadAllTrainCompany();
  }

  initForm() {
    this.updateForm = this.fb.group({
      id: ['', Validators.required],
      trainCompanyId: ['', Validators.required],
      discount: ['', [Validators.required, this.numberValidator()]],
    });

    const id = this.activatedRoute.snapshot.params.id;

    this.roundTripService.getRoundTripById(id)
      .subscribe({
        next: (res) => {
          this.updateForm.patchValue(res);
        },
        error: (err) => {
          this.showToast('danger', 'Failed', 'Round Trip doest not exist!');
          this.router.navigateByUrl('/managements/schedule-and-ticket-prices/round-trip');
        },
      });
  }

  numberValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      if (control.value == null || control.value === '') return null;
      return !isNaN(parseFloat(control.value)) && isFinite(control.value) ? null : { 'notANumber': true };
    };
  }

  onSubmit() {
    if (this.updateForm.valid) {
      this.roundTripService.updateRoundTrip(this.updateForm.value).subscribe({
        next: (res) => {
          this.showToast('success', 'Success', 'Update round trip successfully!');
          this.isSubmitted = false;
          this.errorMessages = [];
        },
        error: (err) => {
          this.errorMessages = err.error.errors;
          this.showToast('danger', 'Failed', 'Update round trip failed!');
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
