import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {environment} from '../../../environments/environment';
import {DashboardDataToday, UpcomingSchedule} from './dashboard.component';

@Injectable({
  providedIn: 'root',
})
export class DashboardService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {
  }

  getDashboardToday() {
    return this.http.get<DashboardDataToday>(this.baseUrl + '/dashboard/dashboardDataToday');
  }

  getUpcomingSchedules() {
    return this.http.get<UpcomingSchedule[]>(this.baseUrl + '/dashboard/upcomingSchedules/5');
  }
}
