import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {environment} from '../../../../environments/environment';
import {Schedule} from '../../../@models/schedule';
import {PaginationService} from '../../shared/pagination.service';
import {ScheduleQueryParams} from '../../../@models/params/scheduleQueryParams';

@Injectable({
  providedIn: 'root',
})
export class ScheduleService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient, private paginationService: PaginationService) {
  }

  getAllSchedule(queryParams: ScheduleQueryParams) {
    let params = this.paginationService
      .getPaginationHeaders(queryParams.pageNumber, queryParams.pageSize);

    if (queryParams.trainId) {
      params = params.append('trainId', queryParams.trainId.toString());
    }

    if (queryParams.departureStationId) {
      params = params.append('departureStationId', queryParams.departureStationId.toString());
    }

    if (queryParams.arrivalStationId) {
      params = params.append('arrivalStationId', queryParams.arrivalStationId.toString());
    }

    if (queryParams.searchTerm) {
      params = params.append('searchTerm', queryParams.searchTerm);
    }

    if (queryParams.sort) {
      params = params.append('sort', queryParams.sort);
    }

    return this.paginationService
      .getPaginatedResult<Schedule[]>(this.baseUrl + '/schedules', params);
  }

  getAllScheduleNoPaging() {
    return this.http.get<Schedule[]>(this.baseUrl + '/schedules/all');
  }

  getScheduleById(id: number) {
    return this.http.get<Schedule>(this.baseUrl + '/schedules/' + id);
  }

  addSchedule(schedule: Schedule) {
    return this.http.post<Schedule>(this.baseUrl + '/schedules', schedule);
  }

  updateSchedule(schedule: Schedule) {
    return this.http.put<Schedule>(this.baseUrl + '/schedules/' + schedule.id, schedule);
  }

  deleteSchedule(id: number) {
    return this.http.patch(this.baseUrl + '/schedules/' + id, {});
  }
}
