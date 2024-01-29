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

export interface Last7DaysSummary {
  ticketPriceSum: number;
  ticketPriceCancelSum: number;
}

@Component({
  selector: 'ngx-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss'],
})

export class DashboardComponent implements OnInit {

  dashboardDataToday: DashboardDataToday | undefined;
  upcomingSchedules: UpcomingSchedule[] = [];
  last7DaysSummary: Last7DaysSummary[] = [];

  constructor(private dashboardService: DashboardService) {
  }

  ngOnInit(): void {
    this.loadDashboardToday();
    this.loadUpcomingSchedules();
    this.loadLast7DaysSummary();
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

  loadLast7DaysSummary() {
    this.dashboardService.getLast7DaysSummary().subscribe((data) => {
      this.last7DaysSummary = data;
    });
  }


}
