import { Injectable } from '@angular/core'
import { HttpClient, HttpParams } from '@angular/common/http'
import { environment } from '../../environments/environment.development'
import { BookingScheduleParams } from '../core/models/params/bookingScheduleParams'
import { Schedule, ScheduleWithBookingParams } from '../core/models/schedule'
import { Passenger, Seat } from '../core/models/trainDetail'
import { PaymentTransaction } from '../core/models/paymentTransaction'

@Injectable({
  providedIn: 'root'
})
export class BookingService {
  baseUrl = environment.apiUrl

  currentBookingScheduleParams: BookingScheduleParams | undefined
  currentSelectSchedule: Schedule | undefined
  currentSelectSeats: Seat[] | undefined
  currentSelectPassengers: Passenger[] | undefined
  currentStep = 1

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

  addTicket(payment: PaymentTransaction) {
    return this.http.post(this.baseUrl + 'booking/payment', payment);
  }

}
