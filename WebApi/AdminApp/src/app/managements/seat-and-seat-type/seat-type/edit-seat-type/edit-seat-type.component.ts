import {Component, OnInit} from '@angular/core';
import {SeatTypeService} from '../seat-type.service';
import {AbstractControl, FormBuilder, FormGroup, ValidationErrors, ValidatorFn, Validators} from '@angular/forms';
import {ActivatedRoute, Router} from '@angular/router';
import {NbGlobalPhysicalPosition, NbToastrService} from '@nebular/theme';


@Component({
  selector: 'ngx-edit-seat-type',
  templateUrl: './edit-seat-type.component.html',
  styleUrls: ['./edit-seat-type.component.scss'],
})
export class EditSeatTypeComponent implements OnInit {
  updateForm: FormGroup = new FormGroup({});
  isSubmitted = false;
  errorMessages: string[] = [];

  constructor(private seatTypeService: SeatTypeService,
              private fb: FormBuilder,
              private activatedRoute: ActivatedRoute,
              private router: Router,
              private toastrService: NbToastrService) {
  }

  ngOnInit(): void {

    this.initForm();
  }

  initForm() {
    const id = this.activatedRoute.snapshot.params.id;

    this.seatTypeService.getSeatTypeById(id)
      .subscribe({
        next: (res) => {
          this.updateForm = this.fb.group({
            id: [res.id, Validators.required],
            name: [res.name, Validators.required],
            serviceCharge: [res.serviceCharge, [Validators.required, Validators.min(0), this.numberValidator()]],
            status: [res.status, Validators.required],
            description: [res.description, Validators.required],
          });
        },
        error: (err) => {
          this.showToast('danger', 'Failed', 'This seat type doest not exist!');
          this.router.navigateByUrl('/managements/seat-and-seat-type/seat-type');
        },
      });

  }

  numberValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      if (control.value == null || control.value === '') return null;
      return !isNaN(parseFloat(control.value)) && isFinite(control.value) ? null : {'notANumber': true};
    };
  }

  onSubmit() {
    this.isSubmitted = true;

    if (this.updateForm.valid) {
      this.seatTypeService.updateSeatType(this.updateForm.value).subscribe({
        next: (res) => {
          this.showToast('success', 'Success', 'Update seat type successfully!');
          this.isSubmitted = false;
          this.errorMessages = [];
        },
        error: (err) => {
          this.showToast('danger', 'Failed', 'Fail to update seat type!');
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
