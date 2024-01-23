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
    // this.generateSeatRows()
  }

  getTrainDetailsByScheduleId(scheduleId: number) {
    this.seatSelectionService.getTrainDetailsByScheduleId(scheduleId).subscribe({
      next: (res) => {
        this.trainDetail = res
        this.carriageMatchType = this.trainDetail.carriages
          .filter(c => c.type.id === this.bookingService.currentSelectSchedule?.selectedSeatType?.id)

        this.currentSelectCarriage = this.carriageMatchType[0]
        this.compartmentOfCarriage = this.currentSelectCarriage?.compartments || []
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

  scrollToTop(): void {
    this.viewportScroller.scrollToPosition([0, 0])
  }

}
