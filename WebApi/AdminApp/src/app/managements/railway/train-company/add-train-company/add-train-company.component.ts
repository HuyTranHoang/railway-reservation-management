import {Component, OnInit} from '@angular/core';
import {TrainCompanyService} from '../train-company.service';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {NbGlobalPhysicalPosition, NbToastrService} from '@nebular/theme';

@Component({
  selector: 'ngx-add-train-company',
  templateUrl: './add-train-company.component.html',
  styleUrls: ['./add-train-company.component.scss'],
})
export class AddTrainCompanyComponent implements OnInit {
  trainCompanyForm: FormGroup = this.fb.group({});

  isSubmitted = false;
  errorMessages: string[] = [];

  logo: File | null = null;

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
      logo: [''],
      status: [''],
    });
  }

  onSubmit() {
    this.isSubmitted = true;
    if (this.trainCompanyForm.valid) {
      this.trainCompanyService.addTrainCompanyWithLogo(this.trainCompanyForm.value, this.logo).subscribe({
        next: (res) => {
          this.showToast('success', 'Success', 'Add train company successfully!');
          this.trainCompanyForm.reset();
          this.isSubmitted = false;
          this.errorMessages = [];
        },
        error: (err) => {
          this.showToast('danger', 'Failed', 'Add train company failed!');
          if (err.error.errorMessages) {
            this.errorMessages = err.error.errorMessages;
          } else {
            this.errorMessages = [err.error.title];
          }
        },
      });
    }
  }

  getFile(event: any) {
    this.logo = event.target.files[0];
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
