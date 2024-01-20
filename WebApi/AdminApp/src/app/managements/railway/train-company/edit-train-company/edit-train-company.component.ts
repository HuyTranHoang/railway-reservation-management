import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {TrainCompanyService} from '../train-company.service';
import {NbGlobalPhysicalPosition, NbToastrService} from '@nebular/theme';
import {ActivatedRoute, Router} from '@angular/router';

@Component({
  selector: 'ngx-edit-train-company',
  templateUrl: './edit-train-company.component.html',
  styleUrls: ['./edit-train-company.component.scss'],
})
export class EditTrainCompanyComponent implements OnInit {
  trainCompanyForm: FormGroup = this.fb.group({});
  isSubmitted: boolean = false;

  logo: File | null = null;


  errorMessages: string[] = [];
  isLoading = false;
  constructor(private trainCompanyService: TrainCompanyService,
              private toastrService: NbToastrService,
              private activatedRoute: ActivatedRoute,
              private router: Router,
              private fb: FormBuilder) {
  }
  ngOnInit(): void {
    this.initForm();
  }

  initForm() {

    this.trainCompanyForm = this.fb.group({
      id: [0, Validators.required],
      name: ['', Validators.required],
      logo: [''],
      status: [''],
    });

    const id = this.activatedRoute.snapshot.params.id;

    this.isLoading = true;
    this.trainCompanyService.getTrainCompanyById(id)
      .subscribe({
        next: (res) => {
          this.trainCompanyForm.patchValue({
            id: res.id,
            name: res.name,
            status: res.status,
          });
          this.isLoading = false;
        },
        error: _ => {
          this.showToast('danger', 'Failed', 'Train Company doest not exist!');
          this.router.navigateByUrl('/managements/railway/train-company');
        },
      });
  }

  onSubmit() {
    this.isSubmitted = true;

    if (this.trainCompanyForm.valid && !this.logo) {
      this.trainCompanyService.updateTrainCompany(this.trainCompanyForm.value).subscribe({
        next: (res) => {
          this.showToast('success', 'Success', 'Update train company successfully!');
          this.isSubmitted = false;
          this.errorMessages = [];
        },
        error: (err) => {
          this.showToast('danger', 'Failed', 'Update train company failed!');
          if (err.error.errorMessages) {
            this.errorMessages = err.error.errorMessages;
          } else {
            this.errorMessages = [err.error.title];
          }
        },
      });
    } else if (this.trainCompanyForm.valid && this.logo) {
      this.trainCompanyService.updateTrainCompanyWithLogo(this.trainCompanyForm.value, this.logo).subscribe({
        next: (res) => {
          this.showToast('success', 'Success', 'Update train company successfully!');
          this.isSubmitted = false;
          this.errorMessages = [];
        },
        error: (err) => {
          this.showToast('danger', 'Failed', 'Update train company failed!');
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
