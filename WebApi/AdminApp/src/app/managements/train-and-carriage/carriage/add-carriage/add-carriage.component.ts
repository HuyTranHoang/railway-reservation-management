import {Component, OnInit, ViewChild} from '@angular/core';
import {Observable, of} from 'rxjs';
import {map} from 'rxjs/operators';
import {TrainService} from '../../train/train.service';
import {Train} from '../../../../@models/train';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {CarriageTypeService} from '../../carriage-type/carriage-type.service';
import {CarriageType} from '../../../../@models/carriageType';
import {CarriageService} from '../carriage.service';
import {NbGlobalPhysicalPosition, NbToastrService} from '@nebular/theme';

@Component({
  selector: 'ngx-add-carriage',
  templateUrl: './add-carriage.component.html',
  styleUrls: ['./add-carriage.component.scss'],
})
export class AddCarriageComponent implements OnInit {

  options: Train[];
  filteredOptions$: Observable<Train[]>;

  carriageTypes: CarriageType[] = [];
  currentCarriageType: CarriageType;
  currentNumberOfSeats: number = 0;

  isValidTrainSelected: boolean = false;

  carriageForm: FormGroup = this.fb.group({});
  isSubmitted = false;
  errorMessages: string[] = [];

  @ViewChild('autoInput') input: { nativeElement: { value: string; }; };


  constructor(private trainService: TrainService,
              private carriageService: CarriageService,
              private carriageTypeService: CarriageTypeService,
              private toastrService: NbToastrService,
              private fb: FormBuilder) {
  }

  ngOnInit() {
    this.loadTrains();
    this.loadCarriageTypes();
    this.initForm();
  }

  private loadTrains() {
    this.trainService.getAllTrainNoPaging().subscribe(trains => {
      this.options = trains;
      this.filteredOptions$ = of(this.options);
    });
  }

  private loadCarriageTypes() {
    this.carriageTypeService.getAllCarriageTypeNoPaging().subscribe({
      next: (res) => {
        this.carriageTypes = res;
      },
    });
  }

  initForm() {
    this.carriageForm = this.fb.group({
      name: ['', [Validators.required]],
      trainId: ['', [Validators.required]],
      carriageTypeId: ['', [Validators.required]],
      status: [''],
    });
  }

  private filter(value: string): Train[] {
    const filterValue = value.toLowerCase();
    return this.options.filter(option => option.name.toLowerCase().includes(filterValue));
  }

  getFilteredOptions(value: string): Observable<Train[]> {
    return of(value).pipe(
      map(filterString => this.filter(filterString)),
    );
  }

  onChange() {
    this.filteredOptions$ = this.getFilteredOptions(this.input.nativeElement.value);
  }

  onSelectionChange(train: Train) {
    this.carriageForm.patchValue({
      trainId: train.id,
    });
    this.input.nativeElement.value = train.name;
    this.isValidTrainSelected = true; // Train hợp lệ được chọn
  }

  onInputBlur() {
    const inputValue = this.input.nativeElement.value;
    const trainExists = this.options.some(train => train.name === inputValue);
    this.isValidTrainSelected = trainExists;
    if (!trainExists) {
      this.carriageForm.patchValue({
        trainId: '',
      });
    }
  }

  firstNextBtnClick() {
    this.isSubmitted = true;

    this.currentCarriageType = this.carriageTypes
      .find(ct => ct.id === this.carriageForm.value.carriageTypeId);

    if (this.currentCarriageType) {
      if (this.currentCarriageType.name.includes('Ngồi mềm điều hòa')) {
        this.currentNumberOfSeats = 32;
      } else if (this.currentCarriageType.name.includes('Giường nằm khoang 4 điều hòa')) {
        this.currentNumberOfSeats = 4;
      } else {
        this.currentNumberOfSeats = 6;
      }
    }
  }

  onSubmit() {
    this.isSubmitted = true;
    this.errorMessages = [];
    if (this.carriageForm.invalid) {
      return;
    }

    this.carriageService.addCarriage(this.carriageForm.value).subscribe({
      next: _ => {
        this.showToast('success', 'Success', 'Add carriage, compartment and seat successfully!');
        this.carriageForm.reset();
        this.isSubmitted = false;
        this.errorMessages = [];
      },
      error: (err) => {
        this.showToast('danger', 'Failed', 'Add carriage, compartment and seat failed!');
        this.errorMessages = err.error.errorMessages;
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
