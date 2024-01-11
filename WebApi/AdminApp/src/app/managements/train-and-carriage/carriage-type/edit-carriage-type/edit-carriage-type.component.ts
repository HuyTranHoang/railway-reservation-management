import {Component, OnInit} from '@angular/core';
import {AbstractControl, FormBuilder, FormGroup, ValidationErrors, ValidatorFn, Validators} from '@angular/forms';
import {CarriageTypeService} from '../carriage-type.service';
import {NbGlobalPhysicalPosition, NbToastrService} from '@nebular/theme';
import {ActivatedRoute, Router} from '@angular/router';

@Component({
  selector: 'ngx-edit-carriage-type',
  templateUrl: './edit-carriage-type.component.html',
  styleUrls: ['./edit-carriage-type.component.scss'],
})
export class EditCarriageTypeComponent implements OnInit {

  carriageTypeForm: FormGroup = this.fb.group({});

  constructor(private carriageTypeService: CarriageTypeService,
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

    this.carriageTypeService.getCarriageTypeById(id)
      .subscribe({
        next: (res) => {
          this.carriageTypeForm = this.fb.group({
            id: [res.id, Validators.required],
            name: [res.name, Validators.required],
            serviceCharge: [res.serviceCharge, [Validators.required, Validators.min(0), this.numberValidator()]],
            status: [res.status],
            description: [res.description, Validators.required],
          });
        },
        error: (err) => {
          this.showToast('danger', 'Failed', 'Carriages type doest not exist!');
          this.router.navigateByUrl('/managements/train-and-carriage/carriage-type');
        },
      });

  }

  numberValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      if (control.value == null || control.value === '') return null;
      return !isNaN(parseFloat(control.value)) && isFinite(control.value) ? null : { 'notANumber': true };
    };
  }

  onSubmit() {
    if (this.carriageTypeForm.valid) {
      this.carriageTypeService.updateCarriageType(this.carriageTypeForm.value).subscribe({
        next: (res) => {
          this.showToast('success', 'Success', 'Update carriage type successfully!');
        },
        error: (err) => {
          this.showToast('danger', 'Failed', 'Update carriage type failed!');
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
