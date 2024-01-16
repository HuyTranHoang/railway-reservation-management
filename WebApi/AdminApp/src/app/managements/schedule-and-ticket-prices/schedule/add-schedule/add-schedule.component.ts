import {TrainStation} from '../../../../@models/trainStation';
import {Component, OnInit} from '@angular/core';
import {AbstractControl, FormBuilder, FormGroup, ValidationErrors, ValidatorFn, Validators} from '@angular/forms';
import {ScheduleService} from '../schedule.service';
import {NbGlobalPhysicalPosition, NbToastrService} from '@nebular/theme';
import {TrainStationService} from '../../../railway/train-station/train-station.service';
import {Train} from '../../../../@models/train';
import {TrainService} from '../../../train-and-carriage/train/train.service';

@Component({
  selector: 'ngx-add-schedule',
  templateUrl: './add-schedule.component.html',
  styleUrls: ['./add-schedule.component.scss'],
})
export class AddScheduleComponent implements OnInit {

  trains: Train[] = [];
  trainStations: TrainStation[] = [];
  scheduleForm: FormGroup = this.fb.group({});
  isSubmitted: boolean = false;
  errorMessages: string[] = [];

  constructor(private scheduleService: ScheduleService,
              private trainService: TrainService,
              private trainStationService: TrainStationService,
              private toastrService: NbToastrService,
              private fb: FormBuilder) {
  }

  ngOnInit(): void {
    this.loadAllTrainAndTrainStation();
    this.initForm();
  }

  initForm() {
    this.scheduleForm = this.fb.group({
      name: ['', Validators.required],
      trainId: ['', Validators.required],
      departureStationId: ['', Validators.required],
      arrivalStationId: ['', Validators.required],
      departureTime: ['', Validators.required],
      status: [''],
    });
  }

  onSubmit() {
    this.isSubmitted = true;
    this.errorMessages = [];

    if (this.scheduleForm.valid) {
      this.scheduleService.addSchedule(this.scheduleForm.value).subscribe({
        next: (res) => {
          this.showToast('success', 'Success', 'Add schedule successfully!');
          this.scheduleForm.reset();
          this.isSubmitted = false;
          this.errorMessages = [];
        },
        error: (err) => {
          this.errorMessages = err.error.errors;
          this.showToast('danger', 'Failed', 'Add schedule failed!');
        },
      });
    }
  }

  loadAllTrainAndTrainStation() {
    this.trainService.getAllTrainNoPaging().subscribe({
      next: (res) => {
        this.trains = res;
      },
    });

    this.trainStationService.getAllTrainStationNoPaging().subscribe({
      next: (res) => {
        this.trainStations = res;
      },
    });
  }

  numberValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      if (control.value == null || control.value === '') return null;
      return !isNaN(parseFloat(control.value)) && isFinite(control.value) ? null : {'notANumber': true};
    };
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
