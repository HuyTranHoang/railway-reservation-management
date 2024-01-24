import { Injectable } from '@angular/core'
import { HttpClient } from '@angular/common/http'
import { environment } from '../../../environments/environment.development'
import { TrainDetail } from '../../core/models/trainDetail'

@Injectable({
  providedIn: 'root'
})
export class SeatSelectionService {

  baseUrl = environment.apiUrl
  constructor(private http: HttpClient) { }


  getTrainDetailsByScheduleId(scheduleId: number) {
    return this.http.get<TrainDetail>(this.baseUrl + 'booking/schedule/' + scheduleId)
  }
}
