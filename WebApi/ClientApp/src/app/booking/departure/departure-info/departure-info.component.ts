import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core'
import { Schedule } from '../../../core/models/schedule'
import { TrainStationService } from '../../../core/services/train-station.service'

@Component({
  selector: 'app-departure-info',
  templateUrl: './departure-info.component.html',
  styleUrls: ['./departure-info.component.scss', '../departure.shared.scss']
})
export class DepartureInfoComponent {
  @Input() scheduleInfo: { fromStationName: string, toStationName: string, departureDate: string } | undefined
  @Output() modifyButtonClicked: EventEmitter<boolean> = new EventEmitter<boolean>()

  isModify = false

  constructor() { }

  modifyButtonClickedHandler() {
    this.isModify = !this.isModify
    this.modifyButtonClicked.emit(this.isModify)
  }

}
