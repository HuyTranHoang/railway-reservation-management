import {Component} from '@angular/core';
import {AbstractControl, FormBuilder, FormGroup, ValidationErrors, ValidatorFn, Validators} from '@angular/forms';
import {NbGlobalPhysicalPosition, NbToastrService} from '@nebular/theme';
import {DistanceFareService} from '../distance-fare.service';
import {TrainCompanyService} from '../../../railway/train-company/train-company.service';
import {TrainCompany} from '../../../../@models/trainCompany';

@Component({
  selector: 'ngx-add-distance-fare',
  templateUrl: './add-distance-fare.component.html',
  styleUrls: ['./add-distance-fare.component.scss']
})
export class AddDistanceFareComponent {
  addForm: FormGroup = this.fb.group({});
  trainCompanies: TrainCompany[] = [];

  isSubmitted = false;
  errorMessages: string[] = [];

  constructor(private distanceFareService: DistanceFareService,
              private trainCompanyService: TrainCompanyService,
              private toastrService: NbToastrService,
              private fb: FormBuilder) {
  }

  ngOnInit(): void {
    this.AddinitForm();
    this.loadAllTrainCompany();
  }

  AddinitForm() {
    this.addForm = this.fb.group({
      status: ['', Validators.required],
      trainCompanyId: [0,[ Validators.required,this.numberValidator()]],
      distance: [0, [Validators.required, this.numberValidator()]],
      price: [0, [Validators.required, this.numberValidator()]],
    });
  }
  loadAllTrainCompany() {
    this.trainCompanyService.getAllTrainCompanyNoPaging().subscribe({
      next: (res: TrainCompany[]) => {
        this.trainCompanies = res;
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

    if (this.addForm.valid) {
      this.distanceFareService.addDistanceFare(this.addForm.value).subscribe({
        next: (res) => {
          this.showToast('success', 'Success', 'Add distance fare successfully!');
          this.addForm.reset();
          this.isSubmitted = false;
          this.errorMessages = [];
        },
        error: (err) => {
          this.showToast('danger', 'Failed', 'Add distance fare failed!');
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
