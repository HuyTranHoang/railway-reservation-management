import { Component, OnInit } from '@angular/core'
import { BookingService } from '../booking.service'
import { ViewportScroller } from '@angular/common'
import { SeatSelectionService } from './seat-selection.service'
import { Carriage, Compartment, Seat, TrainDetail } from '../../core/models/trainDetail'

@Component({
  selector: 'app-seat-selection',
  templateUrl: './seat-selection.component.html',
  styleUrls: ['./seat-selection.component.scss']
})
export class SeatSelectionComponent implements OnInit {

  trainDetail: TrainDetail | undefined

  carriageMatchType: Carriage[] = []
  currentSelectCarriage: Carriage | undefined
  compartmentOfCarriage: Compartment[] = []

  selectedSeat: Seat[] = []

  seatRows: Seat[][] = []

  constructor(public bookingService: BookingService,
              private seatSelectionService: SeatSelectionService,
              private viewportScroller: ViewportScroller) {}

  ngOnInit(): void {
    setTimeout(() => {
      this.bookingService.currentStep = 2
      this.scrollToTop()
    }, 0)

    if (this.bookingService.currentSelectSchedule) {
      this.getTrainDetailsByScheduleId(this.bookingService.currentSelectSchedule.id)
    }
  }

  getTrainDetailsByScheduleId(scheduleId: number) {
    this.seatSelectionService.getTrainDetailsByScheduleId(scheduleId).subscribe({
      next: (res) => {
        this.trainDetail = res
        this.carriageMatchType = this.trainDetail.carriages
          .filter(c => c.type.id === this.bookingService.currentSelectSchedule?.selectedSeatType?.id)

        this.currentSelectCarriage = this.carriageMatchType[0]
        this.compartmentOfCarriage = this.currentSelectCarriage?.compartments || []


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
    })
  }

  toggleSelectSeat(seat: Seat) {
    seat.selected = !seat.selected

    if (seat.selected && !seat.booked) {
      this.selectedSeat.push(seat)
    } else {
      this.selectedSeat = this.selectedSeat.filter(s => s.id !== seat.id)
    }
  }

  selectCarriage(carriage: Carriage) {
    this.currentSelectCarriage = carriage
    this.compartmentOfCarriage = this.currentSelectCarriage?.compartments || []
  }

  scrollToTop(): void {
    this.viewportScroller.scrollToPosition([0, 0])
  }

}
