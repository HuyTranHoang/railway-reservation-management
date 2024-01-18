import {Component, OnInit} from '@angular/core';
import {TrainCompany} from '../../../../@models/trainCompany';
import {AbstractControl, FormBuilder, FormGroup, ValidationErrors, ValidatorFn, Validators} from '@angular/forms';
import {DistanceFareService} from '../distance-fare.service';
import {TrainCompanyService} from '../../../railway/train-company/train-company.service';
import {NbGlobalPhysicalPosition, NbToastrService} from '@nebular/theme';
import {ActivatedRoute, Router} from '@angular/router';

@Component({
  selector: 'ngx-edit-distance-fare',
  templateUrl: './edit-distance-fare.component.html',
  styleUrls: ['./edit-distance-fare.component.scss'],
})
export class EditDistanceFareComponent implements OnInit {
  trainCompanies: TrainCompany[] = [];
  updateForm: FormGroup = this.fb.group({});

  isSubmitted: boolean = false;
  errorMessages: string[] = [];

  isLoading = false;

  constructor(private distanceFareService: DistanceFareService,
              private trainCompanyService: TrainCompanyService,
              private toastrService: NbToastrService,
              private activatedRoute: ActivatedRoute,
              private router: Router,
              private fb: FormBuilder) {
  }


  ngOnInit(): void {
    this.loadAllTrainCompany();
    this.initForm();
  }

  initForm() {
    this.updateForm = this.fb.group({
      id: ['', Validators.required],
      trainCompanyId: ['', Validators.required],
      distance: [0, [Validators.required, this.numberValidator()]],
      price: [0, [Validators.required, this.numberValidator()]],
      status: [''],
    });

    const id = this.activatedRoute.snapshot.params.id;
    this.isLoading = true;
    this.distanceFareService.getDistanceFareById(id)
      .subscribe({
        next: (res) => {
          this.updateForm.patchValue(res);
          this.isLoading = false;
        },
        error: _ => {
          this.showToast('danger', 'Failed', 'distance fare doest not exist!');
          this.router.navigateByUrl('/managements/schedule-and-ticket-prices/distance-fare');
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
    if (this.updateForm.valid) {
      this.distanceFareService.updateDistanceFare(this.updateForm.value).subscribe({
        next: (res) => {
          this.showToast('success', 'Success', 'Update distance fare successfully!');
          this.isSubmitted = false;
          this.errorMessages = [];
        },
        error: (err) => {
          this.errorMessages = err.error.errors;
          this.showToast('danger', 'Failed', 'Update distance fare failed!');
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
      next: (res) => {
        this.trainCompanies = res;
      },
    });
  }
}
