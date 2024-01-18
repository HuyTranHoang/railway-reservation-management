import {Component, OnInit} from '@angular/core';
import {TrainCompany} from '../../../../@models/trainCompany';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {TrainService} from '../train.service';
import {TrainCompanyService} from '../../../railway/train-company/train-company.service';
import {NbGlobalPhysicalPosition, NbToastrService} from '@nebular/theme';
import {ActivatedRoute, Router} from '@angular/router';


@Component({
  selector: 'ngx-edit-train',
  templateUrl: './edit-train.component.html',
  styleUrls: ['./edit-train.component.scss'],
})
export class EditTrainComponent implements OnInit {

  trainCompanies: TrainCompany[] = [];

  updateForm: FormGroup = this.fb.group({});
  isSubmitted: boolean = false;
  errorMessages: string[] = [];

  isLoading = false;

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
    this.isLoading = true;
    this.trainService.getTrainById(id).subscribe({
      next: (res) => {
        this.updateForm.patchValue(res);
        this.isLoading = false;
      },
      error: _ => {
        this.showToast('danger', 'Failed', 'Train does not exist!');
        this.router.navigateByUrl('/managements/train-and-carriage/train');
      },
    });
  }

  onSubmit() {
    this.isSubmitted = true;
    this.errorMessages = [];

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
    this.trainCompanyService.getAllTrainCompanyNoPaging().subscribe({
      next: (res) => {
        this.trainCompanies = res;
      },
    });
  }
}
