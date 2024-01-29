import {Component, OnDestroy} from '@angular/core';
import {NbColorHelper, NbThemeService} from '@nebular/theme';

@Component({
  selector: 'app-barchartjs',
  templateUrl: './barchartjs.component.html',
  styleUrls: ['./barchartjs.component.scss'],
})
export class BarchartjsComponent implements OnDestroy {
  currentDate = new Date();
  dateArray = [];

  data: any;
  options: any;
  themeSubscription: any;

  constructor(private theme: NbThemeService) {
    for (let i = 0; i < 7; i++) {
      let day = this.currentDate.getDate() - i;
      let month = this.currentDate.getMonth() + 1; // Tháng bắt đầu từ 0
      let year = this.currentDate.getFullYear();

      if (day <= 0) {
        const lastMonthDays = new Date(year, month - 1, 0).getDate();
        day += lastMonthDays;
        month -= 1;
        if (month === 0) {
          month = 12;
          year -= 1;
        }
      }

      this.dateArray.unshift(`${month.toString().padStart(2, '0')}/${day.toString().padStart(2, '0')}`);
    }

    this.themeSubscription = this.theme.getJsTheme().subscribe(config => {

      const colors: any = config.variables;
      const chartjs: any = config.variables.chartjs;

      this.data = {
        labels: this.dateArray  ,
        datasets: [{
          data: [65, 59, 80, 81, 56, 55, 40],
          label: 'Revenue',
          backgroundColor: NbColorHelper.hexToRgbA(colors.primaryLight, 0.8),
        }, {
          data: [28, 48, 40, 19, 86, 27, 90],
          label: 'Refund',
          backgroundColor: NbColorHelper.hexToRgbA(colors.infoLight, 0.8),
        }],
      };

      this.options = {
        maintainAspectRatio: false,
        responsive: true,
        legend: {
          labels: {
            fontColor: chartjs.textColor,
          },
        },
        scales: {
          xAxes: [
            {
              gridLines: {
                display: false,
                color: chartjs.axisLineColor,
              },
              ticks: {
                fontColor: chartjs.textColor,
              },
            },
          ],
          yAxes: [
            {
              gridLines: {
                display: true,
                color: chartjs.axisLineColor,
              },
              ticks: {
                fontColor: chartjs.textColor,
              },
            },
          ],
        },
      };
    });
  }

  ngOnDestroy(): void {
    this.themeSubscription.unsubscribe();
  }
}
