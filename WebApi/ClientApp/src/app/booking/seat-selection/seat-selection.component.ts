import { Component, OnInit } from '@angular/core'
import { BookingService } from '../booking.service'
import { ViewportScroller } from '@angular/common'
import { SeatSelectionService } from './seat-selection.service'
import { Carriage, Compartment, Seat, TrainDetail } from '../../core/models/trainDetail'
import { Router } from '@angular/router'
import Swal from 'sweetalert2'
import { SeatType } from '../../core/models/seatType'

@Component({
  selector: 'app-seat-selection',
  templateUrl: './seat-selection.component.html',
  styleUrls: ['./seat-selection.component.scss']
})
export class SeatSelectionComponent implements OnInit {

  seatTypes: SeatType[] = []

  // Departure _de
  trainDetail_de: TrainDetail | undefined
  carriageMatchType_de: Carriage[] = []
  currentSelectCarriage_de: Carriage | undefined
  compartmentOfCarriage_de: Compartment[] = []
  selectedDepartureSeats_de: Seat[] = []
  seatRows_de: Seat[][] = []
  // End of departure

  // Return _re
  trainDetail_re: TrainDetail | undefined
  carriageMatchType_re: Carriage[] = []
  currentSelectCarriage_re: Carriage | undefined
  compartmentOfCarriage_re: Compartment[] = []
  selectedDepartureSeats_re: Seat[] = []
  seatRows_re: Seat[][] = []

  // End of return

  constructor(public bookingService: BookingService,
              private seatSelectionService: SeatSelectionService,
              private router: Router,
              private viewportScroller: ViewportScroller) {}

  ngOnInit(): void {
    setTimeout(() => {
      this.bookingService.currentStep = 2
      this.scrollToTop()
    }, 0)

    this.getSeatTypes()

    if (this.bookingService.currentSelectDepartureSchedule) {
      this.getTrainDetailsByScheduleId()
    }

    if (!this.bookingService.currentBookingScheduleParams) {
      Swal.fire('Oops', 'Please select a valid departure station, arrival station and departure date', 'error')
      this.router.navigate(['/'])
    }
  }

  getSeatTypes() {
    this.seatSelectionService.getAllSeatTypes().subscribe({
      next: (res) => {
        this.seatTypes = res
      }
    })
  }

  getTrainDetailsByScheduleId() {
    const scheduleId_de = this.bookingService.currentSelectDepartureSchedule?.id || 0

    this.seatSelectionService.getTrainDetailsByScheduleId(scheduleId_de).subscribe({
      next: (res) => {
        this.trainDetail_de = res
        this.carriageMatchType_de = this.trainDetail_de.carriages
          .filter(c => c.type.id === this.bookingService.currentSelectDepartureSchedule?.selectedCarriageType?.id)

        this.currentSelectCarriage_de = this.carriageMatchType_de[0]
        this.compartmentOfCarriage_de = this.currentSelectCarriage_de?.compartments || []

        if (this.bookingService.currentSelectDepartureSchedule?.selectedCarriageType?.id == 1)
          this.generateSeatRows_de()
        else { // Cập nhật dữ liệu giường
          this.compartmentOfCarriage_de.forEach(c => {
            c.seats.forEach(s => {
              this.updateSeatDepartureDetails(s)
            })
          })
        }
      }
    })

    if (this.bookingService.isRoundTrip) {
      const scheduleId_re = this.bookingService.currentSelectReturnSchedule?.id || 0
      this.seatSelectionService.getTrainDetailsByScheduleId(scheduleId_re).subscribe({
        next: (res) => {
          this.trainDetail_re = res
          this.carriageMatchType_re = this.trainDetail_re.carriages
            .filter(c => c.type.id === this.bookingService.currentSelectReturnSchedule?.selectedCarriageType?.id)

          this.currentSelectCarriage_re = this.carriageMatchType_re[0]
          this.compartmentOfCarriage_re = this.currentSelectCarriage_re?.compartments || []

          if (this.bookingService.currentSelectReturnSchedule?.selectedCarriageType?.id == 1)
            this.generateSeatRows_re()
          else { // Cập nhật dữ liệu giường
            this.compartmentOfCarriage_re.forEach(c => {
              c.seats.forEach(s => {
                this.updateSeatReturnDetails(s)
              })
            })
          }
        }
      })
    }

  }

  generateSeatRows_de() {
    const seatsPerRow = 8
    const numberOfRows = 4

    for (let i = 0; i < numberOfRows; i++) {
      const row: Seat[] = []

      for (let j = 0; j < seatsPerRow; j++) {
        const seatIndex = i + j * numberOfRows
        if (seatIndex < this.compartmentOfCarriage_de[0].seats.length) {
          this.updateSeatDepartureDetails(this.compartmentOfCarriage_de[0].seats[seatIndex])
          row.push(this.compartmentOfCarriage_de[0].seats[seatIndex])
        }
      }

      this.seatRows_de.push(row)
    }

    for (let i = 0; i < numberOfRows; i++) {
      const row: Seat[] = []

      for (let j = 0; j < seatsPerRow; j++) {
        const seatIndex = i + j * numberOfRows
        if (seatIndex < this.compartmentOfCarriage_de[1].seats.length) {
          this.updateSeatDepartureDetails(this.compartmentOfCarriage_de[1].seats[seatIndex])
          row.push(this.compartmentOfCarriage_de[1].seats[seatIndex])
        }
      }

      this.seatRows_de.push(row)
    }
  }

  generateSeatRows_re() {
    const seatsPerRow = 8
    const numberOfRows = 4

    for (let i = 0; i < numberOfRows; i++) {
      const row: Seat[] = []

      for (let j = 0; j < seatsPerRow; j++) {
        const seatIndex = i + j * numberOfRows
        if (seatIndex < this.compartmentOfCarriage_re[0].seats.length) {
          this.updateSeatReturnDetails(this.compartmentOfCarriage_re[0].seats[seatIndex])
          row.push(this.compartmentOfCarriage_re[0].seats[seatIndex])
        }
      }

      this.seatRows_re.push(row)
    }

    for (let i = 0; i < numberOfRows; i++) {
      const row: Seat[] = []

      for (let j = 0; j < seatsPerRow; j++) {
        const seatIndex = i + j * numberOfRows
        if (seatIndex < this.compartmentOfCarriage_re[1].seats.length) {
          this.updateSeatReturnDetails(this.compartmentOfCarriage_re[1].seats[seatIndex])
          row.push(this.compartmentOfCarriage_re[1].seats[seatIndex])
        }
      }

      this.seatRows_re.push(row)
    }
  }

  toggleSelectDepartureSeat(seat: Seat) {
    seat.selected = !seat.selected

    if (seat.selected && !seat.booked) {
      this.selectedDepartureSeats_de.push(seat)
    } else {
      this.selectedDepartureSeats_de = this.selectedDepartureSeats_de.filter(s => s.id !== seat.id)
    }
  }

  toggleSelectReturnSeat(seat: Seat) {
    seat.selected = !seat.selected

    if (seat.selected && !seat.booked) {
      this.selectedDepartureSeats_re.push(seat)
    } else {
      this.selectedDepartureSeats_re = this.selectedDepartureSeats_re.filter(s => s.id !== seat.id)
    }
  }

  updateSeatDepartureDetails(seat: Seat) {
    seat.carriageId = this.currentSelectCarriage_de?.id || 0
    seat.compartmentId = this.getCompartmentId(seat) || 0
    seat.serviceCharge = this.getServiceCharge(seat) || 0
    seat.seatTypeName = this.getSeatTypeName(seat) || ''

    if (this.bookingService.currentSelectDepartureSchedule) {
      seat.seatTotalPrice = seat.serviceCharge
        + this.bookingService.currentSelectDepartureSchedule.price
        + this.bookingService.currentSelectDepartureSchedule.selectedCarriageType.serviceCharge
    }
  }

  updateSeatReturnDetails(seat: Seat) {
    seat.carriageId = this.currentSelectCarriage_re?.id || 0
    seat.compartmentId = this.getCompartmentId(seat) || 0
    seat.serviceCharge = this.getServiceCharge(seat) || 0
    seat.seatTypeName = this.getSeatTypeName(seat) || ''

    if (this.bookingService.currentSelectReturnSchedule) {
      seat.seatTotalPrice = seat.serviceCharge
        + this.bookingService.currentSelectReturnSchedule.price
        + this.bookingService.currentSelectReturnSchedule.selectedCarriageType.serviceCharge
    }
  }

  getCompartmentId(seat: Seat): number | undefined {
    return this.compartmentOfCarriage_de.find(c => c.seats.find(se => se.id === seat.id))?.id
  }

  getServiceCharge(seat: Seat): number | undefined {
    return this.seatTypes.find(st => st.id === seat.seatTypeId)?.serviceCharge
  }

  getSeatTypeName(seat: Seat): string | undefined {
    return this.seatTypes.find(st => st.id === seat.seatTypeId)?.name
  }

  selectCarriage_de(carriage: Carriage) {
    this.currentSelectCarriage_de = carriage
    this.compartmentOfCarriage_de = this.currentSelectCarriage_de?.compartments || []
    if (this.bookingService.currentSelectDepartureSchedule?.selectedCarriageType?.id == 1) {
      this.seatRows_de = []
      this.generateSeatRows_de()
    }
  }

  selectCarriage_re(carriage: Carriage) {
    this.currentSelectCarriage_re = carriage
    this.compartmentOfCarriage_re = this.currentSelectCarriage_re?.compartments || []
    if (this.bookingService.currentSelectReturnSchedule?.selectedCarriageType?.id == 1) {
      this.seatRows_re = []
      this.generateSeatRows_re()
    }
  }


  onSubmit() {
    if (this.selectedDepartureSeats_de.length === 0) {
      Swal.fire({
        icon: 'error',
        title: 'Oops...',
        text: 'Please select at least one seat for departure'
      })
      return
    }

    if (this.selectedDepartureSeats_de.length > 10) {
      Swal.fire({
        icon: 'error',
        title: 'Oops...',
        text: 'You can only book up to 10 seats'
      })
      return
    }

    this.bookingService.currentSelectSeats = []

    if (!this.bookingService.isRoundTrip)
      this.bookingService.currentSelectSeats = this.selectedDepartureSeats_de
    else {
      if (this.selectedDepartureSeats_re.length === 0) {
        Swal.fire({
          icon: 'error',
          title: 'Oops...',
          text: 'Please select at least one seat for return'
        })
        return
      }

      if (this.selectedDepartureSeats_re.length > 10) {
        Swal.fire({
          icon: 'error',
          title: 'Oops...',
          text: 'You can only book up to 10 seats'
        })
        return
      }

      if (this.selectedDepartureSeats_de.length !== this.selectedDepartureSeats_re.length) {
        Swal.fire({
          icon: 'error',
          title: 'Oops...',
          text: 'Please select the same number of seats for departure and return'
        })
        return
      }

      this.bookingService.currentSelectSeats = [...this.selectedDepartureSeats_de, ...this.selectedDepartureSeats_re]
    }


    this.router.navigate(['/booking/passengers'])
  }

  scrollToTop(): void {
    this.viewportScroller.scrollToPosition([0, 0])
  }

}
