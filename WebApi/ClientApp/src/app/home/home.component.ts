import { Component, ElementRef, OnInit, ViewChild } from '@angular/core'
import { TrainStationService } from '../core/services/train-station.service'
import Swal from 'sweetalert2'
import { Router } from '@angular/router'
import { BookingScheduleParams } from '../core/models/params/bookingScheduleParams'

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss', '../booking/departure/departure.shared.scss']
})
export class HomeComponent implements OnInit {
  @ViewChild('fromSearchInput') fromSearchInput: ElementRef | undefined
  @ViewChild('toSearchInput') toSearchInput: ElementRef | undefined

  originalStations: { id: number, name: string }[] = []

  isFromActivated = false
  isToActivated = false
  fromSearchText = ''
  toSearchText = ''
  fromCurrentStation = 'City, station'
  toCurrentStation = 'City, station'
  fromResultStations = this.originalStations
  toResultStations = this.originalStations

  departureTime = new Date()
  returnDate = new Date()
  currentDate = new Date()

  roundTrip = false

  constructor(private trainStationService: TrainStationService,
              private router: Router) {
  }

  ngOnInit(): void {
    this.trainStationService.getTrainStationsNoPaging().subscribe((res) => {
      this.originalStations = [...res]
      this.fromResultStations = [...res]
      this.toResultStations = [...res]
    })

    this.isFromActivated = false
    this.isToActivated = false
  }


  toggleSelect(isFrom: boolean) {
    if (isFrom) {
      this.isFromActivated = !this.isFromActivated
      this.fromSearchText = ''
      this.resultStationsHandler(this.fromResultStations, this.originalStations)
      this.setFocus(this.fromSearchInput)
    } else {
      this.isToActivated = !this.isToActivated
      this.toSearchText = ''
      this.resultStationsHandler(this.toResultStations, this.originalStations)
      this.setFocus(this.toSearchInput)
    }
  }

  resultStationsHandler(resultStations: any, originalStations: any) {
    if (resultStations.length === 0) {
      resultStations = [...originalStations]
    }
  }

  setFocus(element: ElementRef | undefined) {
    setTimeout(() => {
      element?.nativeElement.focus()
    }, 100)
  }

  updateName(name: string, isFrom: boolean) {
    if (isFrom) {
      this.fromCurrentStation = name
      this.isFromActivated = false
    } else {
      this.toCurrentStation = name
      this.isToActivated = false
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
      isFrom ? this.fromCurrentStation = resultStations[0].name : this.toCurrentStation = resultStations[0].name
      isFrom ? this.isFromActivated = false : this.isToActivated = false
    }
  }

  resetSearch(isFrom: boolean) {
    if (isFrom) {
      this.fromSearchText = ''
      this.fromResultStations = [...this.originalStations]
      this.fromCurrentStation = 'City, station'
    } else {
      this.toSearchText = ''
      this.toResultStations = [...this.originalStations]
      this.toCurrentStation = 'City, station'
    }
  }

  dateChange(newDate: any, isForm: boolean) {
    if (isForm) {
      this.departureTime = newDate
      this.departureTime.setMinutes(this.departureTime.getMinutes() + 1)
    } else {
      this.returnDate = newDate
    }
  }


  toRoundTrip() {
    this.roundTrip = true
  }

  toOneWay() {
    this.roundTrip = false
  }

  onSubmitSearch() {
    const departureStationId = this.originalStations
      .find((station) => station.name === this.fromCurrentStation)?.id

    const arrivalStationId = this.originalStations
      .find((station) => station.name === this.toCurrentStation)?.id

    if (!departureStationId) {
      Swal.fire('Oops', 'Please select a valid departure station', 'error')
      return
    }

    if (!arrivalStationId) {
      Swal.fire('Oops', 'Please select a valid arrival station', 'error')
      return
    }

    if (departureStationId === arrivalStationId) {
      Swal.fire('Oops', 'Departure station and arrival station cannot be the same', 'error')
      return
    }

    if (this.departureTime < this.currentDate) {
      Swal.fire('Oops', 'Departure date cannot be in the past', 'error')
      return
    }

    if (this.roundTrip && !this.returnDate) {
      Swal.fire('Oops', 'Please select a return date', 'error')
      return
    }

    if (this.roundTrip && this.returnDate < this.departureTime) {
      Swal.fire('Oops', 'Return date cannot be before departure date', 'error')
      return
    }

    const departureTime = new Date(this.departureTime.getTime() + (7 * 60 * 60 * 1000)).toUTCString(); // Đúng sau 12h tối
    // const departureTime = this.departureTime.toUTCString() // Đúng sau 6h tối
    let returnDate = null

    if (this.returnDate) {
      returnDate = new Date(this.returnDate.getTime() + (7 * 60 * 60 * 1000)).toUTCString();
      // returnDate = this.returnDate.toUTCString() // Đúng sau 6h tối
    }

    const queryParams: BookingScheduleParams = {
      departureStationId,
      arrivalStationId,
      departureTime,
      returnDate,
      roundTrip: this.roundTrip
    }

    this.router.navigate(['/booking'], { queryParams })
  }

  scroll(el: HTMLElement) {
    const yOffset = 50 // Khoảng cách cần cuộn
    const y = el.getBoundingClientRect().top + window.pageYOffset - yOffset

    window.scrollTo({ top: y, behavior: 'smooth' })
  }
}
