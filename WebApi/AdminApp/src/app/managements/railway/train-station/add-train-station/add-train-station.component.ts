import {Component} from '@angular/core';
import {AbstractControl, FormBuilder, FormGroup, ValidationErrors, ValidatorFn, Validators} from '@angular/forms';
import {TrainStationService} from '../train-station.service';
import {NbGlobalPhysicalPosition, NbToastrService} from '@nebular/theme';

@Component({
  selector: 'ngx-add-train-station',
  templateUrl: './add-train-station.component.html',
  styleUrls: ['./add-train-station.component.scss']
})
export class AddTrainStationComponent {
  addFrom: FormGroup = this.fb.group({});

  isSubmitted = false;
  errorMessages: string[] = [];

  constructor(private seatTypeService: TrainStationService,
              private toastrService: NbToastrService,
              private fb: FormBuilder) {
  }

  ngOnInit(): void {
    this.initForm();
  }

  initForm() {
    this.addFrom = this.fb.group({
      name: ['', Validators.required],
      coordinateValue: [0, [Validators.required, Validators.min(0), this.numberValidator()]],
      status: [''],
      address: ['', Validators.required],
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
    console.log(this.addFrom);

    if (this.addFrom.valid) {
      this.seatTypeService.addTrainStation(this.addFrom.value).subscribe({
        next: (res) => {
          this.showToast('success', 'Success', 'Add train station successfully!');
          this.addFrom.reset();
          this.isSubmitted = false;
          this.errorMessages = [];
        },
        error: (err) => {
          this.showToast('danger', 'Failed', 'Add train station failed!');
          console.log(err);
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
