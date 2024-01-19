import { Component, OnInit } from '@angular/core'
import { BookingService } from '../booking.service'

@Component({
  selector: 'app-seat-selection',
  templateUrl: './seat-selection.component.html',
  styleUrls: ['./seat-selection.component.scss']
})
export class SeatSelectionComponent implements OnInit {

  seatRows: number[][] = [];
  seatStatus: boolean[][] = [];
  constructor(private bookingService: BookingService) {}

  ngOnInit(): void {
    setTimeout(() => {
      this.bookingService.currentStep = 2;
    }, 0);
    this.generateSeatRows();
  }

  generateSeatRows(): void {
    const rows = 4;
    const cols = 8;
    const half = 32;

    // Xử lý cho nửa đầu của các ghế
    for (let row = 0; row < rows; row++) {
      this.seatRows[row] = [];
      this.seatStatus[row] = []; // Khởi tạo mảng trạng thái cho hàng
      for (let col = 0; col < cols; col++) {
        this.seatRows[row][col] = row + 1 + col * rows;
        this.seatStatus[row][col] = false; // Mặc định là ghế chưa đặt
      }
    }

    // Xử lý cho nửa sau của các ghế
    for (let row = 0; row < rows; row++) {
      this.seatRows[row + rows] = []; // Bắt đầu từ hàng thứ 5 (index 4)
      this.seatStatus[row + rows] = []; // Khởi tạo mảng trạng thái cho hàng
      for (let col = 0; col < cols; col++) {
        this.seatRows[row + rows][col] = half + row + 1 + col * rows;
        this.seatStatus[row + rows][col] = false; // Mặc định là ghế chưa đặt
      }
    }
  }

  toggleSeat(row: number, col: number): void {
    this.seatStatus[row][col] = !this.seatStatus[row][col];
  }

}
