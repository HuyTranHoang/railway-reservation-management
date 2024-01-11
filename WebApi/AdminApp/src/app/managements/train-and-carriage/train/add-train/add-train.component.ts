import { TrainService } from './../train.service';
import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { NbGlobalPhysicalPosition, NbToastrService } from '@nebular/theme';
import { TrainCompanyService } from '../../../railway/train-company/train-company.service';
import { TrainCompany } from '../../../../@models/trainCompany';
import { QueryParams } from '../../../../@models/params/queryParams';
import { PaginatedResult } from '../../../../@models/paginatedResult';

@Component({
  selector: 'ngx-add-train',
  templateUrl: './add-train.component.html',
  styleUrls: ['./add-train.component.scss']
})
export class AddTrainComponent implements OnInit {

  trainCompanies : TrainCompany[] = [];

  trainForm: FormGroup = this.fb.group({});
  isSubmitted: boolean = false;
  errorMessages: string[] = [];

  queryParams: QueryParams =
  {
    pageNumber: 1,
    pageSize: 2,
    searchTerm: '',
    sort: '',
  }

  constructor(private trainService: TrainService,
              private trainCompanyService : TrainCompanyService,
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

  numberValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      if (control.value == null || control.value === '') return null;
      return !isNaN(parseFloat(control.value)) && isFinite(control.value) ? null : { 'notANumber': true };
    };
  }

  loadAllTrainCompany(){
    this.trainCompanyService.getAllTrainCompany(this.queryParams).subscribe({
      next: (res : PaginatedResult<TrainCompany[]>) => {
        this.trainCompanies = res.result;
      },
    });
  }
}
