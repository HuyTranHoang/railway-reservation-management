import { Component, OnInit } from "@angular/core";
import { FormGroup, FormBuilder, Validators, ValidatorFn, AbstractControl, ValidationErrors } from "@angular/forms";
import { NbToastrService, NbGlobalPhysicalPosition } from "@nebular/theme";
import { TrainService } from "../train.service";
import { ActivatedRoute, Router } from "@angular/router";
import { TrainCompany } from "../../../../@models/trainCompany";
import { TrainCompanyService } from "../../../railway/train-company/train-company.service";
import { QueryParams } from "../../../../@models/params/queryParams";
import { PaginatedResult } from "../../../../@models/paginatedResult";



@Component({
  selector: 'ngx-edit-train',
  templateUrl: './edit-train.component.html',
  styleUrls: ['./edit-train.component.scss']
})

export class EditTrainComponent implements OnInit{

  trainCompanies : TrainCompany [] = [];

  updateForm: FormGroup = this.fb.group({});
  isSubmitted: boolean = false;
  errorMessages = [];
  queryParams: QueryParams =
  {
    pageNumber: 1,
    pageSize: 999,
    searchTerm: '',
    sort: '',
  }

  constructor(private trainService: TrainService,
    private trainCompanyService : TrainCompanyService,
    private toastrService: NbToastrService,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private fb: FormBuilder) {
}

  ngOnInit(): void {
    this.initForm();
    this.loadAllTrainCompany();
  }

  initForm() {

    this.updateForm = this.fb.group({
      id: ['', Validators.required],
      name: ['', Validators.required],
      trainCompanyId : ['', Validators.required],
      status: [''],
    })
    const id = this.activatedRoute.snapshot.params.id;

    this.trainService.getTrainById(id)
      .subscribe({
        next: (res) => {
          this.updateForm.patchValue(res);
        },
        error: (err) => {
          this.showToast('danger', 'Failed', 'Train doest not exist!');
          this.router.navigateByUrl('/managements/train-and-carriage/train');
        }
      })
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
