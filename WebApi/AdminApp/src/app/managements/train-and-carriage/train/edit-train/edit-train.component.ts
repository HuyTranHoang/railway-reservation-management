import { Component, OnInit } from "@angular/core";
import { FormGroup, FormBuilder, Validators, ValidatorFn, AbstractControl, ValidationErrors } from "@angular/forms";
import { NbToastrService, NbGlobalPhysicalPosition } from "@nebular/theme";
import { TrainService } from "../train.service";
import { ActivatedRoute, Router } from "@angular/router";



@Component({
  selector: 'ngx-edit-train',
  templateUrl: './edit-train.component.html',
  styleUrls: ['./edit-train.component.scss']
})

export class EditTrainComponent implements OnInit{

  trainForm: FormGroup = this.fb.group({});

  constructor(private trainService: TrainService,
    private toastrService: NbToastrService,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private fb: FormBuilder) {
}

  ngOnInit(): void {
    this.initForm();
  }

  initForm() {
    const id = this.activatedRoute.snapshot.params.id;

    this.trainService.getTrainById(id)
      .subscribe({
        next: (res) => {
          this.trainForm = this.fb.group({
            id: [res.id, Validators.required],
            name: [res.name, Validators.required],
            trainCompanyId : [res.trainCompanyId, Validators.required, this.numberValidator()],
            status: [res.status, Validators.required],
          });
        },
        error: (err) => {
          this.showToast('danger', 'Failed', 'Train doest not exist!');
          this.router.navigateByUrl('/managements/train-and-carriage/train');
        }
      })
  }

  onSubmit() {
    if (this.trainForm.valid) {
      this.trainService.updateTrain(this.trainForm.value).subscribe({
        next: (res) => {
          this.showToast('success', 'Success', 'Update train successfully!');
        },
        error: (err) => {
          console.log(this.trainForm.value)
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

}
