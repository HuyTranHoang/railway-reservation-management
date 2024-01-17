import {Component, OnInit} from '@angular/core';

@Component({
  selector: 'app-preview-carriage',
  templateUrl: './preview-carriage.component.html',
  styleUrls: ['./preview-carriage.component.scss'],
})
export class PreviewCarriageComponent implements OnInit {

  seatRows: number[][] = [];

  constructor() {
  }

  ngOnInit(): void {
    this.seatRows = this.generateSeatRows();
  }

  generateSeatRows(): number[][] {
    const seatRows = [];
    const rows = 4; // Số hàng ghế trong mỗi khối (đã được chia đôi)
    const cols = 8; // Số lượng ghế trên mỗi hàng
    const half = 32; // Số ghế trong một nửa của toàn bộ ghế

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
