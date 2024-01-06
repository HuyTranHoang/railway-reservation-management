import { Component } from '@angular/core';

@Component({
  selector: 'app-departure-select',
  templateUrl: './departure-select.component.html',
  styleUrls: ['./departure-select.component.scss']
})
export class DepartureSelectComponent {


  isOpen = false;


  onChevronClick () {
    this.isOpen = !this.isOpen;
  }

}
