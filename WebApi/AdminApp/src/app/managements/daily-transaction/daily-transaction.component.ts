import {Component, OnInit} from '@angular/core';
import {DailyCashTransaction} from '../../@models/dailyCashTransaction';
import {Pagination} from '../../@models/pagination';
import {QueryParams} from '../../@models/params/queryParams';
import {DailyTransactionService} from './daily-transaction.service';
import {NbDialogService, NbGlobalPhysicalPosition, NbToastrService} from '@nebular/theme';
import {PaginatedResult} from '../../@models/paginatedResult';
import {SharedService} from '../shared/shared.service';
import {ExcelService} from './excel.service';
import {ShowExportExcelComponent} from './show-export-excel/show-export-excel.component';

@Component({
  selector: 'ngx-daily-transaction',
  templateUrl: './daily-transaction.component.html',
  styleUrls: ['./daily-transaction.component.scss'],
})
export class DailyTransactionComponent implements OnInit {
  dailyCashTransactions: DailyCashTransaction[] = [];
  dailyCashTransactionsNoPagin: DailyCashTransaction[] = [];
  pagination: Pagination;

  currentSearchTerm: string = '';
  currentSort: string = '';

  sortStates = {
    date: false,
    totalReceived: false,
    totalRefunded: false,
    createdAt: false,
  };

  queryParams: QueryParams = {
    pageNumber: 1,
    pageSize: 10,
    searchTerm: '',
    sort: '',
  };

  constructor(private dailyCashTransactionService: DailyTransactionService,
              private sharedService: SharedService,
              private dialogService: NbDialogService,
              private toastrService: NbToastrService,
              private excelService: ExcelService) {
  }

  ngOnInit(): void {
    this.getAllDailyTransaction();
    this.getAllDailyCashTransactionNoPaging();
  }

  getAllDailyTransaction() {
    this.dailyCashTransactionService.getAllDailyCashTransaction(this.queryParams).subscribe({
      next: (res: PaginatedResult<DailyCashTransaction[]>) => {
        this.dailyCashTransactions = res.result;
        this.pagination = res.pagination;
      },
    });
  }

  getAllDailyCashTransactionNoPaging() {
    this.dailyCashTransactionService.getAllDailyCashTransactionNoPaging().subscribe({
      next: (res: DailyCashTransaction[]) => {
        this.dailyCashTransactionsNoPagin = res;
      },
    });
  }

  onSearch() {
    this.queryParams.searchTerm = this.currentSearchTerm;
    this.queryParams.pageNumber = 1;
    this.getAllDailyTransaction();
  }

  onResetSearch() {
    this.currentSearchTerm = '';
    this.queryParams.searchTerm = '';
    this.getAllDailyTransaction();
  }

  onSort(sort: string) {
    const result = this.sharedService.sortItems(this.queryParams, sort, this.sortStates);
    this.queryParams = result.queryParams;
    this.sortStates = result.sortStates;
    this.currentSort = this.queryParams.sort;
    this.getAllDailyTransaction();
  }

  onPageChanged(newPage: number) {
    this.queryParams.pageNumber = newPage;
    this.getAllDailyTransaction();
  }

  onPageSizeChanged(newSize: number) {
    this.queryParams.pageSize = newSize;
    this.getAllDailyTransaction();
  }

  onFilterFromDate(date: Date) {
    const year = date.getFullYear();
    const month = date.getMonth() + 1; // getMonth() trả về giá trị từ 0 đến 11
    const day = date.getDate();
    this.queryParams.searchTerm = `${year}-${month.toString().padStart(2, '0')}-${day.toString().padStart(2, '0')}`;
    this.getAllDailyTransaction();
  }

  openShowDialog() {
    const dialogRef = this.dialogService.open(ShowExportExcelComponent);

    dialogRef.componentRef.instance.onShowExport.subscribe(obj => {

      if (!obj.startDate || !obj.endDate) {
        this.showToast('danger', 'Failed', 'Please choose start date and end date!');
        return;
      }

      this.exportDataToExcel(obj.startDate, obj.endDate);
    });

  }

  exportDataToExcel(startDate: Date, endDate: Date) {
    const filteredData = this.dailyCashTransactionsNoPagin
      .filter(transaction => new Date(transaction.date) >= startDate
        && new Date(transaction.date) <= endDate);

    if (filteredData.length === 0) {
      this.showToast('danger', 'Failed', 'No data to export!');
      return;
    }

    const headers = ['Date', 'Total Received', 'Total Refunded', 'Created At'];
    const data = filteredData.map(transaction => {
      const transactionDate = new Date(transaction.createdAt);
      const timeString = transactionDate.toLocaleTimeString();
      return [
        transaction.date.split('T')[0],
        transaction.totalReceived,
        transaction.totalRefunded,
        timeString,
      ];
    });

    this.excelService.exportToExcel(data, 'daily_cash_transaction', headers);
  }

  private showToast(type: string, title: string, body: string) {
    const config = {
      status: type,
      destroyByClick: true,
      duration: 3000,
      hasIcon: true,
      position: NbGlobalPhysicalPosition.TOP_RIGHT,
    };
    this.toastrService.show(body, title, config);
  }

}
