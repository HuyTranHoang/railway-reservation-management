import { Component, OnInit } from '@angular/core'
import { BookingService } from '../booking.service'
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms'
import { Router } from '@angular/router'
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker'
import Swal from 'sweetalert2'

@Component({
  selector: 'app-passengers',
  templateUrl: './passengers.component.html',
  styleUrls: ['./passengers.component.scss']
})
export class PassengersComponent implements OnInit {

  passengerForm: FormGroup = new FormGroup({})
  isSubmitted = false

  colorTheme = 'theme-orange';
  bsConfig?: Partial<BsDatepickerConfig> = Object.assign({}, { containerClass: this.colorTheme });

  constructor(public bookingService: BookingService,
              private router: Router,
              private fb: FormBuilder) {}

  ngOnInit(): void {
    setTimeout(() => {
      this.bookingService.currentStep = 3
    }, 0)

    this.passengerForm = this.fb.group({
      passengers: this.fb.array([])
    });


    if (this.bookingService.currentSelectSeats) {
      if(this.bookingService.isRoundTrip)
        for (let i = 0; i < this.bookingService.currentSelectSeats.length/2; i++) {
          this.addPassenger();
        }
      else
        for (let i = 0; i < this.bookingService.currentSelectSeats.length; i++) {
          this.addPassenger();
        }
    }

    if (!this.bookingService.currentBookingScheduleParams) {
      Swal.fire('Oops', 'Please select a valid departure station, arrival station and departure date', 'error')
      this.router.navigate(['/'])
    }
  }

  get passengers(): FormArray {
    return this.passengerForm.get('passengers') as FormArray;
  }

  addPassenger() {
    const passengerGroup = this.fb.group({
      title: ['male', Validators.required], // Giới tính (Mr., Ms., ...)
      fullName: ['', Validators.required],
      passportNumber: ['', Validators.required],
      phoneNumber: ['', [Validators.required, Validators.pattern('^[\\+]?[(]?[0-9]{3}[)]?[-\\s\\.]?[0-9]{3}[-\\s\\.]?[0-9]{4,6}$')]],
      email: ['', Validators.email],
      dob: ['', Validators.required],
    });

    this.passengers.push(passengerGroup);
  }

  removePassenger(index: number) {
    this.passengers.removeAt(index);
  }

  isFieldInvalid(arrayName: string, index: number, fieldName: string): boolean {
    const control = this.passengerForm.get(`${arrayName}.${index}.${fieldName}`);

    if (control) {
      return control && control.invalid;
    }

    return false;
  }

  onSubmit() {
    this.isSubmitted = true
    if (this.passengerForm.invalid) {
      return
    }

    this.isSubmitted = false
    this.bookingService.currentSelectPassengers = []
    this.bookingService.currentSelectPassengers = this.passengerForm.value.passengers
    this.router.navigate(['/booking/payment'])
  }

}
