import {Component, OnInit} from '@angular/core';
import {CarriageTypeService} from '../carriage-type.service';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {NbComponentStatus, NbGlobalPhysicalPosition, NbToastrService} from '@nebular/theme';


@Component({
  selector: 'ngx-add-carriage-type',
  templateUrl: './add-carriage-type.component.html',
  styleUrls: ['./add-carriage-type.component.scss'],
})
export class AddCarriageTypeComponent implements OnInit {
  carriageTypeForm: FormGroup = this.fb.group({});

  constructor(private carriageTypeService: CarriageTypeService,
              private toastrService: NbToastrService,
              private fb: FormBuilder) {
  }

  ngOnInit(): void {
    this.initForm();
  }

  initForm() {
    this.carriageTypeForm = this.fb.group({
      name: ['', Validators.required],
      serviceCharge: [0, [Validators.required, Validators.min(0)]],
      status: ['', Validators.required],
      description: ['', Validators.required],
    });
  }

  onSubmit() {
    if (this.carriageTypeForm.valid) {
      this.carriageTypeService.addCarriageType(this.carriageTypeForm.value).subscribe({
        next: (res) => {
          this.showToast('success', 'Add carriage type successfully!');
          this.carriageTypeForm.reset();
        },
        error: (err) => {
          this.showToast('danger', 'Add carriage type failed!');
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
      position: NbGlobalPhysicalPosition.TOP_RIGHT,
    };
    this.toastrService.show(
      body,
      'Success',
      config);
  }
}
