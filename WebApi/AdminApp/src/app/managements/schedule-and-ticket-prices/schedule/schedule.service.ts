import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { QueryParams } from '../../../@models/params/queryParams';
import { Schedule } from '../../../@models/schedule';
import { PaginationService } from '../../shared/pagination.service';

@Injectable({
  providedIn: 'root'
})
export class ScheduleService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient, private paginationService: PaginationService) {
  }

  getAllSchedule(queryParams: QueryParams) {
    let params = this.paginationService
      .getPaginationHeaders(queryParams.pageNumber, queryParams.pageSize);

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
