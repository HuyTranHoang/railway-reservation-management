import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NbGlobalPhysicalPosition, NbToastrService } from '@nebular/theme';
import { SeatType } from '../../../../@models/seatType';
import { SeatTypeService } from '../../seat-type/seat-type.service';
import { SeatService } from '../seat.service';
import { CompartmentService } from '../../../train-and-carriage/compartment/compartment.service';
import { Compartment } from '../../../../@models/compartment';

@Component({
  selector: 'ngx-add-seat',
  templateUrl: './add-seat.component.html',
  styleUrls: ['./add-seat.component.scss']
})
export class AddSeatComponent implements OnInit {

  seatTypes: SeatType[] = [];
  compartments : Compartment[] = [];
  seatForm: FormGroup = this.fb.group({});
  isSubmitted: boolean = false;
  errorMessages: string[] = [];

  constructor(private seatService: SeatService,
              private seatTypeService: SeatTypeService,
              private compartmentService : CompartmentService,
              private toastrService: NbToastrService,
              private fb: FormBuilder) {
  }

  ngOnInit(): void {
    this.loadAllSeatTypeAndCompartment();
    this.initForm();
  }

  initForm() {
    this.seatForm = this.fb.group({
      name: ['', Validators.required],
      seatTypeId: ['', Validators.required],
      compartmentId: ['',Validators.required],
      status: [''],
    });
  }

  onSubmit() {
    this.isSubmitted = true;
    this.errorMessages = [];

    if (this.seatForm.valid) {
      this.seatService.addSeat(this.seatForm.value).subscribe({
        next: (res) => {
          this.showToast('success', 'Success', 'Add seat successfully!');
          this.seatForm.reset();
          this.isSubmitted = false;
          this.errorMessages = [];
        },
        error: (err) => {
          this.errorMessages = err.error.errors;
          this.showToast('danger', 'Failed', 'Add seat failed!');
        },
      });
    }
  }

  loadAllSeatTypeAndCompartment() {
    this.seatTypeService.getAllSeatTypeNoPaging().subscribe({
      next: (res) => {
        this.seatTypes = res;
      },
    });

    this.compartmentService.getAllCompartmentNoPaging().subscribe({
      next: (res) => {
        this.compartments = res;
      },
    });

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
