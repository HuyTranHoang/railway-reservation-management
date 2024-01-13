import {Component, OnInit, ViewChild} from '@angular/core';
import {TrainCompany} from '../../../../@models/trainCompany';
import {Observable, of} from 'rxjs';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {TrainService} from '../train.service';
import {TrainCompanyService} from '../../../railway/train-company/train-company.service';
import {NbGlobalPhysicalPosition, NbToastrService} from '@nebular/theme';
import {ActivatedRoute, Router} from '@angular/router';
import {map} from 'rxjs/operators';

@Component({
  selector: 'ngx-edit-train',
  templateUrl: './edit-train.component.html',
  styleUrls: ['./edit-train.component.scss'],
})
export class EditTrainComponent implements OnInit {
  options: TrainCompany[];
  filteredOptions$: Observable<TrainCompany[]>;
  isCompanySelected: boolean = false;

  @ViewChild('autoInput') input: { nativeElement: { value: string; }; };

  updateForm: FormGroup = this.fb.group({});
  isSubmitted: boolean = false;
  errorMessages: string[] = [];

  constructor(
    private trainService: TrainService,
    private trainCompanyService: TrainCompanyService,
    private toastrService: NbToastrService,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private fb: FormBuilder,
  ) {
  }

  ngOnInit(): void {
    this.loadAllTrainCompany();
    this.initForm();
  }

  initForm() {
    this.updateForm = this.fb.group({
      id: ['', Validators.required],
      name: ['', Validators.required],
      trainCompanyId: ['', Validators.required],
      status: [''],
    });

    const id = this.activatedRoute.snapshot.params.id;

    this.trainService.getTrainById(id).subscribe({
      next: (res) => {
        this.updateForm.patchValue(res);

        const matchingCompany = this.options.find(company => company.id === res.trainCompanyId);
        if (matchingCompany) {
          this.input.nativeElement.value = matchingCompany.name;
          this.isCompanySelected = true;
        }
      },
      error: (err) => {
        this.showToast('danger', 'Failed', 'Train does not exist!');
        this.router.navigateByUrl('/managements/train-and-carriage/train');
      },
    });
  }

  onSubmit() {
    if (this.updateForm.valid) {
      this.trainService.updateTrain(this.updateForm.value).subscribe({
        next: (res) => {
          this.showToast('success', 'Success', 'Update train successfully!');
          this.isSubmitted = false;
          this.errorMessages = [];
        },
        error: (err) => {
          this.errorMessages = err.error.errors;
          this.showToast('danger', 'Failed', 'Update train failed!');
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
    this.toastrService.show(body, title, config);
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
      this.updateForm.patchValue({ trainCompanyId: null });
      this.isCompanySelected = false;
    }
  }

  onSelectionChange(trainCompany: TrainCompany) {
    this.isCompanySelected = true;
    this.updateForm.patchValue({ trainCompanyId: trainCompany.id });
    this.input.nativeElement.value = trainCompany.name;
  }
}
