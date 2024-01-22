import { Component, ElementRef, OnInit, ViewChild } from '@angular/core'
import { BookingService } from '../booking.service'
import { ActivatedRoute, Router } from '@angular/router'
import { BookingScheduleParams } from '../../core/models/params/bookingScheduleParams'
import { TrainStationService } from '../../core/services/train-station.service'
import Swal from 'sweetalert2'
import { DepartureService } from './departure.service'
import { Schedule } from '../../core/models/schedule'

@Component({
  selector: 'app-departure',
  templateUrl: './departure.component.html',
  styleUrls: ['./departure.component.scss', './departure.shared.scss']
})
export class DepartureComponent implements OnInit {

  // Departure section
  schedulesParams: BookingScheduleParams | undefined
  isModify = false
  roundTrip = false
  currentDepartureDate = new Date()
  currentReturnDate: Date | undefined
  currentDate = new Date()

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

  // End of departure section

  constructor(private bookingService: BookingService,
              private trainStationService: TrainStationService,
              private departureService: DepartureService,
              private router: Router,
              private activatedRoute: ActivatedRoute) {}

  ngOnInit(): void {
    setTimeout(() => {
      this.bookingService.currentStep = 1
    }, 0)

    this.loadStations()
    this.loadQueryParams()
  }


  // Departure section
  loadQueryParams() {
    this.activatedRoute.queryParams.subscribe(params => {
      const departureStationId = params['departureStationId']
      const arrivalStationId = params['arrivalStationId']
      const departureTime = params['departureTime']
      let returnDate: string | null = null
      const roundTrip = params['roundTrip']

      if (roundTrip) {
        returnDate = params['returnDate']
        if (returnDate) {
          this.currentReturnDate = new Date(returnDate)
        }
      }

      this.schedulesParams = { departureStationId, arrivalStationId, departureTime, returnDate, roundTrip }
      this.bookingService.currentBookingScheduleParams = this.schedulesParams

      if (departureStationId && arrivalStationId && departureTime) {
        this.departureService.loadScheduleInfo(departureStationId, arrivalStationId, departureTime)
        this.fromCurrentStation = this.departureService.scheduleInfo?.fromStationName || 'City, station'
        this.toCurrentStation = this.departureService.scheduleInfo?.toStationName || 'City, station'
      }

      if (this.schedulesParams) {
        this.departureService.loadSchedule(this.schedulesParams)
      }

      if (departureTime) {
        this.currentDepartureDate = new Date(departureTime)
      }

      if (roundTrip) {
        this.roundTrip = roundTrip === 'true'
      }

    })
  }

  loadStations() {
    this.trainStationService.getTrainStationsNoPaging().subscribe((res) => {
      this.originalStations = [...res]
      this.fromResultStations = [...res]
      this.toResultStations = [...res]

    })
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
      this.currentDepartureDate = newDate
      this.currentDepartureDate.setMinutes(this.currentDepartureDate.getMinutes() + 1)
    } else {
      this.currentReturnDate = newDate
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

    if (this.currentDepartureDate < this.currentDate) {
      Swal.fire('Oops', 'Departure date cannot be in the past', 'error')
      return
    }

    if (this.roundTrip && this.currentReturnDate && this.currentReturnDate < this.currentDepartureDate) {
      Swal.fire('Oops', 'Return date cannot be before departure date', 'error')
      return
    }

    const departureTime = new Date(this.currentDepartureDate.getTime() + (7 * 60 * 60 * 1000)).toISOString();
    let returnDate = null

    if (this.currentReturnDate) {
      returnDate = new Date(this.currentReturnDate.getTime() + (7 * 60 * 60 * 1000)).toISOString();
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


  onModifyButtonClicked(isModified: boolean) {
    this.isModify = isModified
  }

  onBookNowClick(currentSelectSchedule: Schedule) {
    this.bookingService.currentSelectSchedule = currentSelectSchedule
    this.router.navigate(['/booking/seat-selection'])
  }

  // End of departure section

}
