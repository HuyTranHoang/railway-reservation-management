import {Component, OnInit} from '@angular/core';
import { DailyCashTransaction } from '../../@models/dailyCashTransaction';
import { Pagination } from '../../@models/pagination';
import { QueryParams } from '../../@models/params/queryParams';
import { DailyTransactionService } from './daily-transaction.service';
import {NbDialogService} from '@nebular/theme';
import { PaginatedResult } from '../../@models/paginatedResult';
import { SharedService } from '../shared/shared.service';
import { ExcelService } from './excel.service';

@Component({
  selector: 'ngx-daily-transaction',
  templateUrl: './daily-transaction.component.html',
  styleUrls: ['./daily-transaction.component.scss'],
})
export class DailyTransactionComponent implements OnInit {
  dailyCashTransactions: DailyCashTransaction[] = [];
  dailyCashTransactionsNoPagin: DailyCashTransaction[] = [];
  pagination: Pagination;

  startDate: string = '';
  endDate: string = '';


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

  exportDataToExcel() {
    if (!this.startDate || !this.endDate) {
      alert('Please enter both start and end dates.');

      this.startDate = '';
      this.endDate = '';

      return;
    }
    const headers = ['Date', 'Total Received', 'Total Refunded', 'Created At'];
    const data = this.dailyCashTransactionsNoPagin
      .filter(transaction => transaction.date.split('T')[0] >= this.startDate && transaction.date.split('T')[0] <= this.endDate)
      .map(transaction => {
        return [
          transaction.date.split('T')[0],
          transaction.totalReceived,
          transaction.totalRefunded,
          transaction.createdAt
        ];
      });
    
    this.excelService.exportToExcel(data, 'daily_cash_transaction', headers);
  }
}
