import { Component, EventEmitter, Output } from '@angular/core'

@Component({
  selector: 'app-departure-info',
  templateUrl: './departure-info.component.html',
  styleUrls: ['./departure-info.component.scss', '../departure.shared.scss']
})
export class DepartureInfoComponent {

  isModify = false;
  @Output() modifyButtonClicked:EventEmitter<boolean> = new EventEmitter<boolean>();

  modifyButtonClickedHandler() {
    this.isModify = !this.isModify;
    this.modifyButtonClicked.emit(this.isModify);
  }

}
