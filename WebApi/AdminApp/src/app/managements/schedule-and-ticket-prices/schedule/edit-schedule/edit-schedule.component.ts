import {Component, OnInit} from '@angular/core';
import {AbstractControl, FormBuilder, FormGroup, ValidationErrors, ValidatorFn, Validators} from '@angular/forms';
import {NbGlobalPhysicalPosition, NbToastrService} from '@nebular/theme';
import {Train} from '../../../../@models/train';
import {TrainStation} from '../../../../@models/trainStation';
import {TrainStationService} from '../../../railway/train-station/train-station.service';
import {TrainService} from '../../../train-and-carriage/train/train.service';
import {ScheduleService} from '../schedule.service';
import {ActivatedRoute, Router} from '@angular/router';

@Component({
  selector: 'ngx-edit-schedule',
  templateUrl: './edit-schedule.component.html',
  styleUrls: ['./edit-schedule.component.scss'],
})
export class EditScheduleComponent implements OnInit {

  trains: Train[] = [];
  trainStations: TrainStation[] = [];
  updateForm: FormGroup = this.fb.group({});
  isSubmitted: boolean = false;
  errorMessages: string[] = [];

  constructor(private scheduleService: ScheduleService,
              private trainService: TrainService,
              private trainStationService: TrainStationService,
              private toastrService: NbToastrService,
              private activatedRoute: ActivatedRoute,
              private router: Router,
              private fb: FormBuilder) {
  }

  ngOnInit(): void {
    this.loadAllTrainAndTrainStation();
    this.initForm();
  }

  initForm() {
    this.updateForm = this.fb.group({
      id: ['', Validators.required],
      name: ['', Validators.required],
      trainId: ['', Validators.required],
      departureStationId: ['', Validators.required],
      arrivalStationId: ['', Validators.required],
      departureTime: ['', Validators.required],
      status: [''],
    });


    const id = this.activatedRoute.snapshot.params.id;

    this.scheduleService.getScheduleById(id).subscribe({
      next: (res) => {
        this.updateForm.patchValue(res);
        this.updateForm.patchValue({
          departureTime: new Date(res.departureTime),
        });
      },
      error: () => {
        this.showToast('danger', 'Failed', 'Schedule does not exist!');
        this.router.navigateByUrl('/managements/schedule-and-ticket-prices/schedule');
      },
    });
  }

  onSubmit() {
    this.isSubmitted = true;
    this.errorMessages = [];

    if (this.updateForm.valid) {
      this.scheduleService.addSchedule(this.updateForm.value).subscribe({
        next: (res) => {
          this.showToast('success', 'Success', 'Add schedule successfully!');
          this.updateForm.reset();
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
