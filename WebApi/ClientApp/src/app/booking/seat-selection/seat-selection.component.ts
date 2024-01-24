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

  trainDetail: TrainDetail | undefined

  carriageMatchType: Carriage[] = []
  currentSelectCarriage: Carriage | undefined
  compartmentOfCarriage: Compartment[] = []

  selectedSeats: Seat[] = []

  seatRows: Seat[][] = []

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

    if (this.bookingService.currentSelectSchedule) {
      this.getTrainDetailsByScheduleId(this.bookingService.currentSelectSchedule.id)
    }
  }

  getSeatTypes() {
    this.seatSelectionService.getAllSeatTypes().subscribe({
      next: (res) => {
        this.seatTypes = res
      }
    })
  }

  getTrainDetailsByScheduleId(scheduleId: number) {
    this.seatSelectionService.getTrainDetailsByScheduleId(scheduleId).subscribe({
      next: (res) => {
        this.trainDetail = res
        this.carriageMatchType = this.trainDetail.carriages
          .filter(c => c.type.id === this.bookingService.currentSelectSchedule?.selectedCarriageType?.id)

        this.currentSelectCarriage = this.carriageMatchType[0]
        this.compartmentOfCarriage = this.currentSelectCarriage?.compartments || []

        if (this.bookingService.currentSelectSchedule?.selectedCarriageType?.id == 1)
          this.generateSeatRows()
      }
    })
  }

  generateSeatRows() {
    const seatsPerRow = 8;
    const numberOfRows = 4

    for (let i = 0; i < numberOfRows; i++) {
      const row: Seat[] = [];

      for (let j = 0; j < seatsPerRow; j++) {
        const seatIndex = i + j * numberOfRows;
        if (seatIndex < this.compartmentOfCarriage[0].seats.length) {
          row.push(this.compartmentOfCarriage[0].seats[seatIndex]);
        }
      }

      this.seatRows.push(row);
    }

    for (let i = 0; i < numberOfRows; i++) {
      const row: Seat[] = [];

      for (let j = 0; j < seatsPerRow; j++) {
        const seatIndex = i + j * numberOfRows;
        if (seatIndex < this.compartmentOfCarriage[1].seats.length) {
          row.push(this.compartmentOfCarriage[1].seats[seatIndex]);
        }
      }

      this.seatRows.push(row);
    }
  }

  toggleSelectSeat(seat: Seat) {
    seat.selected = !seat.selected

    if (seat.selected && !seat.booked) {
      this.selectedSeats.push(seat)
    } else {
      this.selectedSeats = this.selectedSeats.filter(s => s.id !== seat.id)
    }
  }

  selectCarriage(carriage: Carriage) {
    this.currentSelectCarriage = carriage
    this.compartmentOfCarriage = this.currentSelectCarriage?.compartments || []
  }

  onSubmit() {

    if (this.selectedSeats.length === 0) {
      Swal.fire({
        icon: 'error',
        title: 'Oops...',
        text: 'Please select at least one seat'
      })
      return
    }

    this.selectedSeats.forEach(s => {
      s.serviceCharge = this.seatTypes.find(st => st.id === s.seatTypeId)?.serviceCharge || 0
      s.seatTypeName = this.seatTypes.find(st => st.id === s.seatTypeId)?.name || ''
    })

    this.bookingService.currentSelectSeats = []
    this.bookingService.currentSelectSeats = this.selectedSeats
    this.router.navigate(['/booking/passengers'])
  }

  scrollToTop(): void {
    this.viewportScroller.scrollToPosition([0, 0])
  }

}
