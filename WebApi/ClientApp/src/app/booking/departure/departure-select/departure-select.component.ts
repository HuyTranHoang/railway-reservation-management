import { Component, EventEmitter, Output } from '@angular/core'

@Component({
  selector: 'app-departure-select',
  templateUrl: './departure-select.component.html',
  styleUrls: ['./departure-select.component.scss']
})
export class DepartureSelectComponent {

  @Output() bookNow = new EventEmitter<number>();

  isOpen = false;

  onChevronClick () {
    this.isOpen = !this.isOpen;
  }

  onBookNowClick(id: number) {
    this.bookNow.emit(id);
  }

}
