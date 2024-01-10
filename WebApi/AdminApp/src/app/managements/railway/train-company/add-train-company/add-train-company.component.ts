import {Component, OnInit} from '@angular/core';
import {TrainCompanyService} from '../train-company.service';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {NbToastrService, NbGlobalPhysicalPosition} from '@nebular/theme';

@Component({
  selector: 'ngx-add-train-company',
  templateUrl: './add-train-company.component.html',
  styleUrls: ['./add-train-company.component.scss'],
})
export class AddTrainCompanyComponent implements OnInit {
  trainCompanyForm: FormGroup = this.fb.group({});

  isSubmitted = false;
  errorMessages: string[] = [];

  constructor(
    private trainCompanyService: TrainCompanyService,
    private fb: FormBuilder,
    private toastrService: NbToastrService,
  ) {
  }

  ngOnInit(): void {
    this.initForm();
  }

  initForm() {
    this.trainCompanyForm = this.fb.group({
      name: ['', Validators.required],
      status: [''],
    });
  }

  onSubmit() {
    this.isSubmitted = true;
    if (this.trainCompanyForm.valid) {
      this.trainCompanyService.addTrainCompany(this.trainCompanyForm.value).subscribe({
        next: (res) => {
          this.showToast('success', 'Success', 'Add train company successfully!');
          this.trainCompanyForm.reset();
          this.isSubmitted = false;
          this.errorMessages = [];
        },
        error: (err) => {
          this.showToast('danger', 'Failed', 'Add train company failed!');
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
