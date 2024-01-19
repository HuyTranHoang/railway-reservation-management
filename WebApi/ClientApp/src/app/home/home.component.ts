import { Component, ElementRef, OnInit, ViewChild } from '@angular/core'
import { TrainStationService } from '../core/services/train-station.service'

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss', '../booking/departure/departure.shared.scss']
})
export class HomeComponent implements OnInit {
  @ViewChild('fromSearchInput') fromSearchInput: ElementRef | undefined
  @ViewChild('toSearchInput') toSearchInput: ElementRef | undefined

  originalStations: {id: number, name: string}[] = []

  isFromActivated = false
  isToActivated = false
  fromSearchText = ''
  toSearchText = ''
  fromCurrentStation = 'City, station'
  toCurrentStation = 'City, station'
  fromResultStations = this.originalStations
  toResultStations = this.originalStations

  constructor(private trainStationService: TrainStationService) {
  }

  ngOnInit(): void {
    this.trainStationService.getTrainStationsNoPaging().subscribe((res) => {
      this.originalStations = [...res]
      this.fromResultStations = [...res]
      this.toResultStations = [...res]
    })
  }


  toggleSelect(isFrom: boolean) {
    if (isFrom) {
      this.isFromActivated = !this.isFromActivated;
      this.fromSearchText = '';
      this.resultStationsHandler(this.fromResultStations, this.originalStations);
      this.setFocus(this.fromSearchInput);
    } else {
      this.isToActivated = !this.isToActivated;
      this.toSearchText = '';
      this.resultStationsHandler(this.toResultStations, this.originalStations);
      this.setFocus(this.toSearchInput);
    }
  }

  resultStationsHandler(resultStations: any, originalStations: any) {
    if (resultStations.length === 0) {
      resultStations = [...originalStations];
    }
  }

  setFocus(element: ElementRef | undefined) {
    setTimeout(() => {
      element?.nativeElement.focus();
    }, 100);
  }

  updateName(name: string, isFrom: boolean) {
    if (isFrom) {
      this.fromCurrentStation = name;
      this.isFromActivated = false;
    } else {
      this.toCurrentStation = name;
      this.isToActivated = false;
    }
  }

  searchStation(name: any, isFrom: boolean) {
    let arr: any = []
    let searchVal = name.target.value
    if (searchVal && searchVal.trim() != '') {
      arr = this.originalStations.filter((item) => {
        return (item.name.toLowerCase().indexOf(searchVal.toLowerCase()) > -1)
      })
      if (isFrom) {
        this.fromResultStations = arr
      } else {
        this.toResultStations = arr
      }
    } else {
      if (isFrom) {
        this.fromResultStations = [...this.originalStations]
      } else {
        this.toResultStations = [...this.originalStations]
      }
    }
  }

  selectFirstInResult(resultStations: any[], currentStation: string, isFrom: boolean) {
    if (resultStations.length > 0 && resultStations[0].name !== 'No Record Found') {
      isFrom ? this.fromCurrentStation = resultStations[0].name : this.toCurrentStation = resultStations[0].name;
      isFrom ? this.isFromActivated = false : this.isToActivated = false;
    }
  }

  resetSearch(isFrom: boolean) {
    if (isFrom) {
      this.fromSearchText = '';
      this.fromResultStations = [...this.originalStations];
      this.fromCurrentStation = 'City, station';
    } else {
      this.toSearchText = '';
      this.toResultStations = [...this.originalStations];
      this.toCurrentStation = 'City, station';
    }
  }
}
