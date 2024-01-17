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
  isSubmitted = false;
  errorMessages: string[] = [];

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

    this.carriageTypeForm = this.fb.group({
      id: [0, Validators.required],
      name: ['', Validators.required],
      numberOfCompartments: [0, [Validators.required, Validators.min(1), this.numberValidator()]],
      serviceCharge: [0, [Validators.required, Validators.min(0), this.numberValidator()]],
      status: [''],
      description: ['', Validators.required],
    });

    const id = this.activatedRoute.snapshot.params.id;

    this.carriageTypeService.getCarriageTypeById(id)
      .subscribe({
        next: (res) => {
          this.carriageTypeForm.patchValue(res);
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
    this.isSubmitted = true;
    this.errorMessages = [];

    if (this.carriageTypeForm.valid) {
      this.carriageTypeService.updateCarriageType(this.carriageTypeForm.value).subscribe({
        next: (res) => {
          this.showToast('success', 'Success', 'Update carriage type successfully!');
          this.isSubmitted = false;
          this.errorMessages = [];
        },
        error: (err) => {
          this.showToast('danger', 'Failed', 'Update carriage type failed!');
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
