import { Injectable } from '@angular/core'
import { environment } from 'src/environments/environment'
import { Ticket } from '../core/models/trainStaion copy'
import { HttpClient } from '@angular/common/http'
import { Schedule } from '../core/models/schedule'

@Injectable({
  providedIn: 'root'
})
export class ManageBookingService {
  baseUrl = environment.apiUrl

  constructor(private http: HttpClient) {
  }

  getLookUpByEmailAndCode(code: string, email: string) {
    return this.http.get<Ticket>(this.baseUrl + 'lookup/' + code + '/' + email)
  }

  getScheduleById(id: number) {
    return this.http.get<Schedule>(this.baseUrl + 'schedules/' + id)
  }
}
