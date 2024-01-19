import { Component, OnInit } from '@angular/core'
import { BookingService } from '../booking.service'

@Component({
  selector: 'app-seat-selection',
  templateUrl: './seat-selection.component.html',
  styleUrls: ['./seat-selection.component.scss']
})
export class SeatSelectionComponent implements OnInit {

  seatRows: number[][] = [];
  constructor(private bookingService: BookingService) {}

  ngOnInit(): void {
    setTimeout(() => {
      this.bookingService.currentStep = 2;
    }, 0);
    this.seatRows = this.generateSeatRows();
  }

  generateSeatRows(): number[][] {
    const seatRows: number[][] = [];
    const rows = 4;
    const cols = 8;
    const half = 32;

    // Xử lý cho nửa đầu của các ghế
    for (let row = 0; row < rows; row++) {
      seatRows[row] = [];
      for (let col = 0; col < cols; col++) {
        seatRows[row][col] = row + 1 + col * rows;
      }
    }

    // Xử lý cho nửa sau của các ghế
    for (let row = 0; row < rows; row++) {
      seatRows[row + rows] = []; // Bắt đầu từ hàng thứ 5 (index 4)
      for (let col = 0; col < cols; col++) {
        seatRows[row + rows][col] = half + row + 1 + col * rows;
      }
    }

    return seatRows;
  }

}
