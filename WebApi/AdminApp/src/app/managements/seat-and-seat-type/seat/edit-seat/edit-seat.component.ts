import {Component, OnInit} from '@angular/core';
import {FormGroup, FormBuilder, Validators} from '@angular/forms';
import {NbToastrService, NbGlobalPhysicalPosition} from '@nebular/theme';
import {Compartment} from '../../../../@models/compartment';
import {SeatType} from '../../../../@models/seatType';
import {CompartmentService} from '../../../train-and-carriage/compartment/compartment.service';
import {SeatTypeService} from '../../seat-type/seat-type.service';
import {SeatService} from '../seat.service';
import {ActivatedRoute, Router} from '@angular/router';

@Component({
  selector: 'ngx-edit-seat',
  templateUrl: './edit-seat.component.html',
  styleUrls: ['./edit-seat.component.scss'],
})
export class EditSeatComponent implements OnInit {

  seatTypes: SeatType[] = [];
  compartments: Compartment[];

  updateForm: FormGroup = this.fb.group({});
  isSubmitted: boolean = false;
  errorMessages: string[] = [];

  constructor(private seatService: SeatService,
              private seatTypeService: SeatTypeService,
              private compartmentService: CompartmentService,
              private activatedRoute: ActivatedRoute,
              private router: Router,
              private toastrService: NbToastrService,
              private fb: FormBuilder) {
  }

  ngOnInit(): void {
    this.loadAllSeatTypeAndCompartment();
    this.initForm();
  }

  initForm() {
    this.updateForm = this.fb.group({
      id: ['', Validators.required],
      name: ['', Validators.required],
      seatTypeId: ['', Validators.required],
      compartmentId: ['', Validators.required],
      status: [''],
    });

    const id = this.activatedRoute.snapshot.params.id;

    this.seatService.getSeatById(id).subscribe({
      next: (res) => {
        this.updateForm.patchValue(res);
      },
      error: (err) => {
        this.showToast('danger', 'Failed', 'Seat does not exist!');
        this.router.navigateByUrl('/managements/seat-and-seat-type/seat');
      },
    });
  }

  onSubmit() {
    this.isSubmitted = true;
    this.errorMessages = [];

    if (this.updateForm.valid) {
      this.seatService.addSeat(this.updateForm.value).subscribe({
        next: (res) => {
          this.showToast('success', 'Success', 'Update seat successfully!');
          this.updateForm.reset();
          this.isSubmitted = false;
          this.errorMessages = [];
        },
        error: (err) => {
          this.errorMessages = err.error.errors;
          this.showToast('danger', 'Failed', 'Update seat failed!');
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
