import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http'
import { environment } from '../../environments/environment.development'
import { BookingScheduleParams } from '../core/models/params/bookingScheduleParams'
import { Schedule, ScheduleWithBookingParams } from '../core/models/schedule'
import { Seat } from '../core/models/trainDetail'

@Injectable({
  providedIn: 'root'
})
export class BookingService {
  baseUrl = environment.apiUrl

  currentBookingScheduleParams: BookingScheduleParams | undefined
  currentSelectSchedule: Schedule | undefined
  currentSelectSeat: Seat[] | undefined
  currentStep = 1;

  constructor(private http: HttpClient) { }

  getBookingSchedule(queryParams: BookingScheduleParams) {
    let params = new HttpParams()

    if (queryParams.departureStationId) {
      params = params.append('departureStationId', queryParams.departureStationId)
    }

    if (queryParams.arrivalStationId) {
      params = params.append('arrivalStationId', queryParams.arrivalStationId)
    }

    if (queryParams.departureTime) {
      params = params.append('departureTime', queryParams.departureTime)
    }

    if (queryParams.roundTrip) {
      params = params.append('roundTrip', queryParams.roundTrip)
    }

    return this.http.get<ScheduleWithBookingParams>(this.baseUrl + 'booking/schedule', { params })
  }

}
