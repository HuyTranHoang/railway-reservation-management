import {Component} from '@angular/core';

@Component({
  selector: 'app-preview-carriage',
  templateUrl: './preview-carriage.component.html',
  styleUrls: ['./preview-carriage.component.scss'],
})
export class PreviewCarriageComponent {
  // Tạo ghế
  seats: any[] = [];

  constructor() {
    for (let i = 1; i <= 64; i++) {
      this.seats.push(i);
    }
  }
}
