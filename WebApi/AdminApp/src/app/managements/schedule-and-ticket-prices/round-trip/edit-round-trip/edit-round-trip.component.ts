import {Component, OnInit} from '@angular/core';
import {AbstractControl, FormBuilder, FormGroup, ValidationErrors, ValidatorFn, Validators} from '@angular/forms';
import { RoundTripService } from '../round-trip.service';
import {NbGlobalPhysicalPosition, NbToastrService} from '@nebular/theme';
import {ActivatedRoute, Router} from '@angular/router';
import { RoundTrip } from '../../../../@models/roundTrip';
import { TrainCompanyService } from '../../../railway/train-company/train-company.service';
import { QueryParams } from '../../../../@models/params/queryParams';
import { PaginatedResult } from '../../../../@models/paginatedResult';
import { TrainCompany } from '../../../../@models/trainCompany';


@Component({
  selector: 'ngx-edit-round-trip',
  templateUrl: './edit-round-trip.component.html',
  styleUrls: ['./edit-round-trip.component.scss']
})
export class EditRoundTripComponent implements OnInit{
  roundTripForm: FormGroup = this.fb.group({});
  roundTrips: RoundTrip[] = [];

  trainCompanies: TrainCompany[] = [];

  updateForm: FormGroup = this.fb.group({});

  isSubmitted: boolean = false;
  errorMessages = [];

  constructor(private roundTripService: RoundTripService,
              private trainCompanyService : TrainCompanyService,
              private toastrService: NbToastrService,
              private activatedRoute: ActivatedRoute,
              private router: Router,
              private fb: FormBuilder) {
  }

  queryParams: QueryParams =
  {
    pageNumber: 1,
    pageSize: 999,
    searchTerm: '',
    sort: '',
  }

  ngOnInit(): void {
    this.initForm();
    this.loadAllTrainCompany();
  }

  initForm() {
    this.updateForm = this.fb.group({
      id: ['', Validators.required],
      trainCompanyId : ['', Validators.required],
      discount: ['', Validators.required],
    })

    const id = this.activatedRoute.snapshot.params.id;

    this.roundTripService.getRoundTripById(id)
    .subscribe({
      next: (res) => {
        this.updateForm.patchValue(res);
      },
      error: (err) => {
        this.showToast('danger', 'Failed', 'Round Trip doest not exist!');
        this.router.navigateByUrl('/managements/schedule-and-ticket-prices/round-trip');
      }
    })
  }

  onSubmit() {
    if (this.updateForm.valid) {
      this.roundTripService.updateRoundTrip(this.updateForm.value).subscribe({
        next: (res) => {
          this.showToast('success', 'Success', 'Update round trip successfully!');
          this.isSubmitted = false;
          this.errorMessages = [];
        },
        error: (err) => {
          this.errorMessages = err.error.errors;
          this.showToast('danger', 'Failed', 'Update round trip failed!');
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
}
