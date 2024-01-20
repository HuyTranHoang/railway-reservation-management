import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http'
import { environment } from '../../environments/environment.development'
import { BookingScheduleParams } from '../core/models/params/bookingScheduleParams'
import { Schedule } from '../core/models/schedule'

@Injectable({
  providedIn: 'root'
})
export class BookingService {
  baseUrl = environment.apiUrl

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

    if (queryParams.departureDate) {
      params = params.append('departureDate', queryParams.departureDate)
    }

    if (queryParams.roundTrip) {
      params = params.append('roundTrip', queryParams.roundTrip)
    }

    return this.http.get<Schedule[]>(this.baseUrl + 'booking/schedule', { params })
  }

}
