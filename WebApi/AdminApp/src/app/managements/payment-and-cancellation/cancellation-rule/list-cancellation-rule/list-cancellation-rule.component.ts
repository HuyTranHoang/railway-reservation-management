import {Component, OnInit} from '@angular/core';
import {CancellationRuleService} from '../cancellation-rule.service';
import {CancellationRule} from '../../../../@models/cancellationRule';
import {Pagination} from '../../../../@models/pagination';
import {PaginatedResult} from '../../../../@models/paginatedResult';
import {QueryParams} from '../../../../@models/params/queryParams';
import {ShowCancellationRuleComponent} from '../show-cancellation-rule/show-cancellation-rule.component';
import {
  ConfirmDeleteCancellationRuleComponent,
} from '../confirm-delete-cancellation-rule/confirm-delete-cancellation-rule.component';
import {NbDialogService} from '@nebular/theme';
import {SharedService} from '../../../shared/shared.service';


@Component({
  selector: 'ngx-list-cancellation-rule',
  templateUrl: './list-cancellation-rule.component.html',
  styleUrls: ['./list-cancellation-rule.component.scss'],
})
export class ListCancellationRuleComponent implements OnInit {
  cancellationRules: CancellationRule[] = [];
  pagination: Pagination;

  currentSearchTerm: string = '';
  currentSort: string = '';

  queryParams: QueryParams = {
    pageNumber: 1,
    pageSize: 10,
    searchTerm: '',
    sort: '',
  };

  sortStates = {
    departureDateDifference: false,
    fee: false,
    status: false,
    createdAt: false,
  };

  constructor(private cancellationRuleService: CancellationRuleService,
              private sharedService: SharedService,
              private dialogService: NbDialogService) {
  }

  ngOnInit(): void {
    this.getAllCancellationRule();
  }

  getAllCancellationRule() {
    this.cancellationRuleService.getAllCancellationRule(this.queryParams).subscribe({
      next: (res: PaginatedResult<CancellationRule[]>) => {
        this.cancellationRules = res.result;
        this.pagination = res.pagination;

        this.checkItemsAndAdjustPage();
      },
    });
  }

  checkItemsAndAdjustPage() {
    if (this.cancellationRules.length === 0 && this.pagination.currentPage > 1) {
      this.queryParams.pageNumber = this.pagination.currentPage - 1;
      this.getAllCancellationRule();
    }
  }

  onSearch() {
    this.queryParams.searchTerm = this.currentSearchTerm;
    this.queryParams.pageNumber = 1;
    this.getAllCancellationRule();
  }

  onResetSearch() {
    this.currentSearchTerm = '';
    this.queryParams.searchTerm = '';
    this.getAllCancellationRule();
  }

  onSort(sort: string) {
    const result = this.sharedService.sortItems(this.queryParams, sort, this.sortStates);
    this.queryParams = result.queryParams;
    this.sortStates = result.sortStates;
    this.currentSort = this.queryParams.sort;
    this.getAllCancellationRule();
  }


  openShowDialog(id: number) {
    const cancellationRule = this.cancellationRules.find(x => x.id === id);

    const dialogRef = this.dialogService.open(ShowCancellationRuleComponent, {
      context: {...cancellationRule},
    });

    dialogRef.componentRef.instance.onShowDelete.subscribe(obj => {
      this.openConfirmDialog(obj.id, obj.departureDateDifference);
    });
  }

  openConfirmDialog(id: number, departureDateDifference: number) {
    const dialogRef = this.dialogService.open(ConfirmDeleteCancellationRuleComponent, {
      context: {
        id,
        departureDateDifference,
      },
    });

    dialogRef.componentRef.instance.onConfirmDelete.subscribe(_ => {
      this.getAllCancellationRule();
    });
  }

  onPageChanged(newPage: number) {
    this.queryParams.pageNumber = newPage;
    this.getAllCancellationRule();
  }

  onPageSizeChanged(newSize: number) {
    this.queryParams.pageSize = newSize;
    this.getAllCancellationRule();
  }
}
