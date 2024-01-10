import { Component, Input } from '@angular/core';
import { SeatTypeService } from '../seat-type.service';
import { AbstractControl, FormBuilder, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SeatType } from '../../../../@models/seatType';
import { timeStamp } from 'console';
import { NbGlobalPhysicalPosition, NbToastrService } from '@nebular/theme';


@Component({
  selector: 'ngx-edit-seat-type',
  templateUrl: './edit-seat-type.component.html',
  styleUrls: ['./edit-seat-type.component.scss']
})
export class EditSeatTypeComponent {
  seatType : SeatType | undefined ;
  @Input() initialValue: any; // Giá trị khởi tạo từ component cha
  updateForm: FormGroup = new FormGroup({
  
  });
  
  seatTypeId: number | undefined;
  constructor(private seatTypeService : SeatTypeService,private fb: FormBuilder,private activatedRoute : ActivatedRoute,
    private toastrService: NbToastrService,) {
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
          this.showToast('danger', 'Failed');
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
    if (this.updateForm.valid) {
      this.seatTypeService.updateSeatType(this.updateForm.value).subscribe({
        next: (res) => {
          this.showToast('success', 'Success');
        },
        error: (err) => {
          this.showToast('danger', 'Failed');
        },
      });
    }
  }
  getDate(){
    const currentDate = new Date();
    const formattedDate: string = currentDate.toISOString();
    return formattedDate;
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
