import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment'
import { HttpClient } from '@angular/common/http'
import { TrainStation } from '../models/trainStaion'

@Injectable({
  providedIn: 'root'
})
export class TrainStationService {
  baseUrl = environment.apiUrl
  constructor(private http: HttpClient) { }

  getTrainStationsNoPaging() {
    return this.http.get<TrainStation[]>(this.baseUrl + 'trainStation/all')
  }
}
