import { TrainService } from './../train.service';
import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { NbGlobalPhysicalPosition, NbToastrService } from '@nebular/theme';

@Component({
  selector: 'ngx-add-train',
  templateUrl: './add-train.component.html',
  styleUrls: ['./add-train.component.scss']
})
export class AddTrainComponent implements OnInit {

  // trainCompanies : TrainCompany[] = [];

  trainForm: FormGroup = this.fb.group({});

  constructor(private trainService: TrainService,
              private toastrService: NbToastrService,
              private fb: FormBuilder) {
  }

  ngOnInit(): void {
    this.initForm();
  }

  initForm() {
    this.trainForm = this.fb.group({
      name: ['', Validators.required],
      trainCompanyId: [1, [Validators.required, this.numberValidator()]],
      status: ['', Validators.required],
    });
  }

  onSubmit() {
    if (this.trainForm.valid) {
      this.trainService.addTrain(this.trainForm.value).subscribe({
        next: (res) => {
          this.showToast('success', 'Success', 'Add train successfully!');
          this.trainForm.reset();
        },
        error: (err) => {
          console.log(this.trainForm.value)
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

  // loadTrainCompany() {
  //   this.trainCompanyService.getTrainCompany().subscribe({
  //     next: (response) => {
  //       this.trainCompany = response
  //     },
  //     error: (error) => {
  //       console.log(error)
  //     }
  //   })
  // }
}
