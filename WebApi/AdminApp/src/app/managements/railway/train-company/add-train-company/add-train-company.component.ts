import { Component, OnInit } from "@angular/core";
import { TrainCompanyService } from "../train-company.service";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { NbToastrService, NbGlobalPhysicalPosition } from "@nebular/theme";

@Component({
  selector: "ngx-add-train-company",
  templateUrl: "./add-train-company.component.html",
  styleUrls: ["./add-train-company.component.scss"],
})
export class AddTrainCompanyComponent implements OnInit {
  trainCompanyForm : FormGroup = this.fb.group({});

  constructor(
    private trainCompanyService : TrainCompanyService,
    private fb : FormBuilder,
    private toastrService : NbToastrService,
  ) {}

  ngOnInit(): void {
    this.initForm();
  }

  initForm(){
    this.trainCompanyForm = this.fb.group({
      name: ['', Validators.required],
    });
  }

  onSubmit(){
    if (this.trainCompanyForm.valid) {
      this.trainCompanyService.addTrainCompany(this.trainCompanyForm.value).subscribe({
        next: (res) => {
          this.showToast('success', 'Add train company successfully!');
          this.trainCompanyForm.reset();
        },
        error: (err) => {
          this.showToast('danger', 'Add train company failed!');
        },
      });
    }
  }

  private showToast(type: string, body: string) {
    const config = {
      status: type,
      destroyByClick: true,
      duration: 2000,
      hasIcon: true,
      position: NbGlobalPhysicalPosition.TOP_LEFT,
    };

    this.toastrService.show(
      body,
      'Success',
      config);
  }
}
