import {Component, Input, OnChanges, OnDestroy, SimpleChanges} from '@angular/core';
import {NbColorHelper, NbThemeService} from '@nebular/theme';
import {Last7DaysSummary} from '../dashboard.component';

@Component({
  selector: 'app-barchartjs',
  templateUrl: './barchartjs.component.html',
  styleUrls: ['./barchartjs.component.scss'],
})
export class BarchartjsComponent implements OnChanges, OnDestroy {
  @Input() last7DaysSummary: Last7DaysSummary[] = [];

  currentDate = new Date();
  dateArray = [];

  data: any;
  options: any;
  themeSubscription: any;

  config: any;

  constructor(private theme: NbThemeService) {
    this.generateDateArray();
    this.setupChart();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.last7DaysSummary) {
      this.convertDataForChart();
    }
  }

  private generateDateArray(): void {
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
  }

  private convertDataForChart(): void {
    // Tạo một mảng mới với đúng 7 phần tử và chèn giá trị 0 vào phía trước nếu cần
    const revenueData = this.fillArrayWithZeros(this.last7DaysSummary
      .map(x => (x.ticketPriceSum / 1000000).toFixed(2)));
    const refundData = this.fillArrayWithZeros(this.last7DaysSummary
      .map(x => (x.ticketPriceCancelSum / 1000000).toFixed(2)));

    const colors: any = this.config.variables;
    this.data = {
      labels: this.dateArray,
      datasets: [
        {
          data: revenueData,
          label: 'Revenue (Million VND)',
          backgroundColor: NbColorHelper.hexToRgbA(colors.primaryLight, 0.8),
        },
        {
          data: refundData,
          label: 'Refund (Million VND)',
          backgroundColor: NbColorHelper.hexToRgbA(colors.infoLight, 0.8),
        },
      ],
    };
  }

  private fillArrayWithZeros(dataArray: string[]): number[] {
    const numberOfZerosToAdd = 7 - dataArray.length;
    const filledArray = Array(numberOfZerosToAdd).fill(0).concat(dataArray.map(Number));
    return filledArray.slice(-7); // Giữ lại chỉ 7 phần tử cuối cùng
  }


  private setupChart(): void {
    this.themeSubscription = this.theme.getJsTheme().subscribe(config => {

      const chartjs: any = config.variables.chartjs;
      this.config = config;

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
