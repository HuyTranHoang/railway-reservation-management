import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
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

  isLoading = false;

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
      arrivalTime: ['', Validators.required],
      status: [''],
    });


    const id = this.activatedRoute.snapshot.params.id;
    this.isLoading = true;
    this.scheduleService.getScheduleById(id).subscribe({
      next: (res) => {
        this.updateForm.patchValue(res);
        this.updateForm.patchValue({
          departureTime: new Date(res.departureTime),
        });
        this.isLoading = false;
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
      this.scheduleService.updateSchedule(this.updateForm.value).subscribe({
        next: (res) => {
          this.showToast('success', 'Success', 'Update schedule successfully!');
          this.isSubmitted = false;
          this.errorMessages = [];
        },
        error: (err) => {
          this.errorMessages = err.error.errors;
          this.showToast('danger', 'Failed', 'Update schedule failed!');
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
