import {Component, Input, OnInit} from '@angular/core';
import {AbstractControl, FormBuilder, FormGroup, ValidationErrors, ValidatorFn, Validators} from '@angular/forms';
import {TrainStationService} from '../train-station.service';
import {ActivatedRoute, Router} from '@angular/router';
import {NbGlobalPhysicalPosition, NbToastrService} from '@nebular/theme';

@Component({
  selector: 'ngx-edit-train-station',
  templateUrl: './edit-train-station.component.html',
  styleUrls: ['./edit-train-station.component.scss'],
})
export class EditTrainStationComponent implements OnInit {
  updateForm: FormGroup = new FormGroup({});

  constructor(private trainStationService: TrainStationService,
              private fb: FormBuilder,
              private activatedRoute: ActivatedRoute,
              private router: Router,
              private toastrService: NbToastrService) {
  }

  ngOnInit(): void {
    this.initForm();
  }

  initForm() {
    this.updateForm = this.fb.group({
      id: [0, Validators.required],
      name: ['', Validators.required],
      coordinateValue: [0, [Validators.required, Validators.min(0), this.numberValidator()]],
      status: [''],
      address: ['', Validators.required],
    });

    const id = this.activatedRoute.snapshot.params.id;

    this.trainStationService.getTrainStationById(id)
      .subscribe({
        next: (res) => {
          this.updateForm.patchValue(res);
        },
        error: (err) => {
          this.showToast('danger', 'Failed', 'This train station doest not exist!');
          this.router.navigateByUrl('/managements/railway/train-station');
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
      this.trainStationService.updateTrainStation(this.updateForm.value).subscribe({
        next: _ => {
          this.showToast('success', 'Success', 'Update train station successfully!');
        },
        error: _ => {
          this.showToast('danger', 'Failed', 'Update train station failed!');
        },
      });
    }
  }

  getDate() {
    const currentDate = new Date();
    const formattedDate: string = currentDate.toISOString();
    return formattedDate;
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
