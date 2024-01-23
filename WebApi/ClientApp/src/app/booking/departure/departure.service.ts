import { Injectable } from '@angular/core'
import { TrainStationService } from '../../core/services/train-station.service'
import { Schedule } from '../../core/models/schedule'
import { BookingService } from '../booking.service'
import { BookingScheduleParams } from '../../core/models/params/bookingScheduleParams'

@Injectable({
  providedIn: 'root'
})
export class DepartureService {

  schedules: Schedule[] = []
  scheduleInfo: { fromStationName: string, toStationName: string, departureDate: string } | undefined

  constructor(private trainStationService: TrainStationService,
              private bookingService: BookingService) { }

  loadScheduleInfo(departureStationId: number, arrivalStationId: number, departureDate: string) {
    let fromStationName = '', toStationName = ''

    this.trainStationService.getStationById(departureStationId.toString())
      .subscribe((res) => {
        fromStationName = res.name
        this.trainStationService.getStationById(arrivalStationId.toString())
          .subscribe((res) => {
            toStationName = res.name
            this.scheduleInfo = { fromStationName, toStationName, departureDate }
          })
      })
  }

  loadSchedule(scheduleParams: BookingScheduleParams) {

    this.bookingService.getBookingSchedule(scheduleParams).subscribe({
      next: (schedules: Schedule[]) => {
        this.schedules = schedules
      },
      error: (err: any) => {
        console.log(err)
      }
    })
  }
}
