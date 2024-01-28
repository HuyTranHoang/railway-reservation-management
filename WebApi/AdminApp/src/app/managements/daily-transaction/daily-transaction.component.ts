import {Component, OnInit} from '@angular/core';
import { DailyCashTransaction } from '../../@models/dailyCashTransaction';
import { Pagination } from '../../@models/pagination';
import { QueryParams } from '../../@models/params/queryParams';
import { DailyTransactionService } from './daily-transaction.service';
import {NbDialogService} from '@nebular/theme';
import { PaginatedResult } from '../../@models/paginatedResult';
import { SharedService } from '../shared/shared.service';

@Component({
  selector: 'ngx-daily-transaction',
  templateUrl: './daily-transaction.component.html',
  styleUrls: ['./daily-transaction.component.scss'],
})
export class DailyTransactionComponent implements OnInit {
  dailyCashTransactions: DailyCashTransaction[] = [];
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
              private dialogService: NbDialogService) {
  }

  ngOnInit(): void {
    this.getAllDailyTransaction();
  }

  getAllDailyTransaction() {
    this.dailyCashTransactionService.getAllDailyCashTransaction(this.queryParams).subscribe({
      next: (res: PaginatedResult<DailyCashTransaction[]>) => {
        this.dailyCashTransactions = res.result;
        this.pagination = res.pagination;
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
}
