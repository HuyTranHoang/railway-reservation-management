import { Component, OnInit } from '@angular/core'
import { BookingService } from '../booking.service'
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms'

@Component({
  selector: 'app-passengers',
  templateUrl: './passengers.component.html',
  styleUrls: ['./passengers.component.scss']
})
export class PassengersComponent implements OnInit {

  passengerForm: FormGroup = new FormGroup({})
  isSubmitted = false

  constructor(public bookingService: BookingService,
              private fb: FormBuilder) {}

  ngOnInit(): void {
    setTimeout(() => {
      this.bookingService.currentStep = 3
    }, 0)

    this.passengerForm = this.fb.group({
      passengers: this.fb.array([])
    });

    for (const item of this.bookingService.currentSelectSeats!) {
      this.addPassenger();
    }
  }

  get passengers(): FormArray {
    return this.passengerForm.get('passengers') as FormArray;
  }

  addPassenger() {
    const passengerGroup = this.fb.group({
      title: ['—'], // Giới tính (Mr., Ms., ...)
      surname: ['', Validators.required],
      givenName: ['', Validators.required],
      passportNumber: ['', Validators.required]
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
    // this.router.navigate(['/booking/passengers'])
  }

}
