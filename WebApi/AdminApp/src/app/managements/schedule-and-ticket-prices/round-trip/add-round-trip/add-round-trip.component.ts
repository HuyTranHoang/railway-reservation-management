import {Component, OnInit} from '@angular/core';
import { RoundTripService } from '../round-trip.service';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {NbToastrService, NbGlobalPhysicalPosition} from '@nebular/theme';
import { RoundTrip } from '../../../../@models/roundTrip';
import { QueryParams } from '../../../../@models/params/queryParams';
import { PaginatedResult } from '../../../../@models/paginatedResult';
import { TrainCompany } from '../../../../@models/trainCompany';
import { TrainCompanyService } from '../../../railway/train-company/train-company.service';

@Component({
  selector: 'ngx-add-round-trip',
  templateUrl: './add-round-trip.component.html',
  styleUrls: ['./add-round-trip.component.scss']
})
export class AddRoundTripComponent implements OnInit{
  roundTrips: RoundTrip[] = [];
  trainCompanies: TrainCompany[] = [];

  roundTripForm: FormGroup = this.fb.group({});

  isSubmitted = false;
  errorMessages: string[] = [];

  queryParams: QueryParams =
  {
    pageNumber: 1,
    pageSize: 999,
    searchTerm: '',
    sort: '',
  }

  constructor(
    private roundTripService: RoundTripService,
    private trainCompanyService: TrainCompanyService,
    private fb: FormBuilder,
    private toastrService: NbToastrService,
  ) {
  }

  ngOnInit(): void {
    this.initForm();
    this.loadAllTrainCompany();
  }

  initForm() {
    this.roundTripForm = this.fb.group({
      trainCompanyId: ['', Validators.required],
      discount: ['', Validators.required],
    });
  }

  onSubmit() {
    this.isSubmitted = true;
    if (this.roundTripForm.valid) {
      this.roundTripService.addRoundTrip(this.roundTripForm.value).subscribe({
        next: (res) => {
          this.showToast('success', 'Success', 'Add round trip successfully!');
          this.roundTripForm.reset();
          this.isSubmitted = false;
          this.errorMessages = [];
        },
        error: (err) => {
          this.showToast('danger', 'Failed', 'Add round trip failed!');
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

  loadAllTrainCompany(){
    this.trainCompanyService.getAllTrainCompany(this.queryParams).subscribe({
      next: (res : PaginatedResult<TrainCompany[]>) => {
        this.trainCompanies = res.result;
      },
    });
  }

  onTrainCompanyNameChange(event: string) {
    this.roundTripForm.patchValue({
      trainCompanyName: event,
    });
  }
}
