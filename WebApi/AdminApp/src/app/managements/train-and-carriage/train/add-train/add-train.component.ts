import { TrainService } from '../train.service';
import {Component, OnInit, ViewChild} from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NbGlobalPhysicalPosition, NbToastrService } from '@nebular/theme';
import { TrainCompanyService } from '../../../railway/train-company/train-company.service';
import { TrainCompany } from '../../../../@models/trainCompany';
import { map } from 'rxjs/operators';
import {Observable, of} from 'rxjs';

@Component({
  selector: 'ngx-add-train',
  templateUrl: './add-train.component.html',
  styleUrls: ['./add-train.component.scss'],
})
export class AddTrainComponent implements OnInit {

  options: TrainCompany[];
  filteredOptions$: Observable<TrainCompany[]>;
  isCompanySelected: boolean = false;

  @ViewChild('autoInput') input: { nativeElement: { value: string; }; };

  trainForm: FormGroup = this.fb.group({});
  isSubmitted: boolean = false;
  errorMessages: string[] = [];

  constructor(private trainService: TrainService,
              private trainCompanyService: TrainCompanyService,
              private toastrService: NbToastrService,
              private fb: FormBuilder) {
  }

  ngOnInit(): void {
    this.initForm();
    this.loadAllTrainCompany();
  }

  initForm() {
    this.trainForm = this.fb.group({
      name: ['', Validators.required],
      trainCompanyId: ['', Validators.required],
      status: [''],
    });
  }

  onSubmit() {
    this.isSubmitted = true;
    this.errorMessages = [];

    if (!this.isCompanySelected && this.input.nativeElement.value !== '') {
      this.errorMessages.push('Please select a valid train company.');
      return;
    }

    if (this.trainForm.valid) {
      this.trainService.addTrain(this.trainForm.value).subscribe({
        next: (res) => {
          this.showToast('success', 'Success', 'Add train successfully!');
          this.trainForm.reset();
          this.isSubmitted = false;
          this.errorMessages = [];
        },
        error: (err) => {
          this.errorMessages = err.error.errors;
          this.showToast('danger', 'Failed', 'Add train failed!');
        },
      });
    }
  }

  loadAllTrainCompany() {
    this.trainCompanyService.getAllTrainCompanyNoPaging().subscribe(res => {
      this.options = res;
      this.filteredOptions$ = of(this.options);
    });
  }

  private filter(value: string): TrainCompany[] {
    const filterValue = value.toLowerCase();
    return this.options.filter(option => option.name.toLowerCase().includes(filterValue));
  }

  getFilteredOptions(value: string): Observable<TrainCompany[]> {
    return of(value).pipe(
      map(filterString => this.filter(filterString)),
    );
  }

  onChange() {
    this.isCompanySelected = false;
    this.errorMessages = [];
    this.filteredOptions$ = this.getFilteredOptions(this.input.nativeElement.value);
  }

  onBlur() {
    const inputValue = this.input.nativeElement.value;
    const matchingCompany = this.options.find(company => company.name === inputValue);

    if (!matchingCompany) {
      this.trainForm.patchValue({ trainCompanyId: null });
      this.isCompanySelected = false;
    }
  }

  onSelectionChange(trainCompany: TrainCompany) {
    this.isCompanySelected = true;
    this.trainForm.patchValue({ trainCompanyId: trainCompany.id });
    this.input.nativeElement.value = trainCompany.name;
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
