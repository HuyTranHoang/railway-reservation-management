import {Component, OnInit, ViewChild} from '@angular/core';
import {Observable, of} from 'rxjs';
import {map} from 'rxjs/operators';
import {TrainService} from '../../train/train.service';
import {Train} from '../../../../@models/train';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {CarriageTypeService} from '../../carriage-type/carriage-type.service';
import {CarriageType} from '../../../../@models/carriageType';

@Component({
  selector: 'ngx-add-carriage',
  templateUrl: './add-carriage.component.html',
  styleUrls: ['./add-carriage.component.scss'],
})
export class AddCarriageComponent implements OnInit {

  options: Train[];
  filteredOptions$: Observable<Train[]>;

  carriageTypes: CarriageType[] = [];

  isValidTrainSelected: boolean = false;

  carriageForm: FormGroup = this.fb.group({});
  isSubmitted = false;
  errorMessages: string[] = [];

  @ViewChild('autoInput') input: { nativeElement: { value: string; }; };

  constructor(private trainService: TrainService,
              private carriageTypeService: CarriageTypeService,
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
      numberOfCompartments: [0, [Validators.required, Validators.min(1)]],
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
  }

}
