import {Component, OnInit} from '@angular/core';
import {DashboardService} from './dashboard.service';


export interface DashboardDataToday {
  userRegistered: number;
  ticketSold: number;
  totalRevenue: number;
  refundAmount: number;
}

export interface UpcomingSchedule {
  scheduleId: number;
  scheduleName: string;
  departureTime: string;
  totalSeats: number;
  seatsBooked: number;
  seatsAvailable: number;
}


@Component({
  selector: 'ngx-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss'],
})

export class DashboardComponent implements OnInit {

  dashboardDataToday: DashboardDataToday;
  upcomingSchedules: UpcomingSchedule[] = [];

  constructor(private dashboardService: DashboardService) {
  }

  ngOnInit(): void {
    this.loadDashboardToday();
    this.loadUpcomingSchedules();
  }

  loadDashboardToday() {
    this.dashboardService.getDashboardToday().subscribe((data) => {
      this.dashboardDataToday = data;
    });
  }

  loadUpcomingSchedules() {
    this.dashboardService.getUpcomingSchedules().subscribe((data) => {
      this.upcomingSchedules = data;
    });
  }


}
