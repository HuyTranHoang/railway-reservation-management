import {Component, OnInit} from '@angular/core';
import { CancellationService } from '../cancellation.service';
import { Cancellation } from '../../../../@models/cancellation';
import {Pagination} from '../../../../@models/pagination';
import {PaginatedResult} from '../../../../@models/paginatedResult';
import { CancellationQueryParams } from '../../../../@models/params/cancellationQueryParams';
import { ShowCancellationComponent } from '../show-cancellation/show-cancellation.component';
import { ConfirmDeleteCancellationComponent } from '../confirm-delete-cancellation/confirm-delete-cancellation.component';
import {NbDialogService} from '@nebular/theme';
import {SharedService} from '../../../shared/shared.service';

@Component({
  selector: 'ngx-list-cancellation',
  templateUrl: './list-cancellation.component.html',
  styleUrls: ['./list-cancellation.component.scss']
})
export class ListCancellationComponent implements OnInit{
  cancellations: Cancellation[] = [];
  pagination: Pagination;

  currentSearchTerm: string = '';
  currentSort: string = '';

  queryParams: CancellationQueryParams = {
    pageNumber: 1,
    pageSize: 10,
    searchTerm: '',
    sort: '',
    ticketId: null,
    cancellationRuleId: null,
  };

  sortStates = {
    ticketCode: false,
    cancellationRuleDepartureDateDifference: false,
    cancellationRuleFee: false,
    reason: false,
    createdAt: false,
  };

  constructor(private cancellationService: CancellationService,
              private sharedService: SharedService,
              private dialogService: NbDialogService) {
  }

  ngOnInit(): void {
    this.getAllCancellation();
  }

  getAllCancellation() {
    this.cancellationService.getAllCancellation(this.queryParams).subscribe({
      next: (res: PaginatedResult<Cancellation[]>) => {
        this.cancellations = res.result;
        this.pagination = res.pagination;

        this.checkItemsAndAdjustPage();
      },
    });
  }

  checkItemsAndAdjustPage() {
    if (this.cancellations.length === 0 && this.pagination.currentPage > 1) {
      this.queryParams.pageNumber = this.pagination.currentPage - 1;
      this.getAllCancellation();
    }
  }

  onSearch() {
    this.queryParams.searchTerm = this.currentSearchTerm;
    this.queryParams.pageNumber = 1;
    this.getAllCancellation();
  }

  onResetSearch() {
    this.currentSearchTerm = '';
    this.queryParams.searchTerm = '';
    this.getAllCancellation();
  }

  onSort(sort: string) {
    const result = this.sharedService.sortItems(this.queryParams, sort, this.sortStates);
    this.queryParams = result.queryParams;
    this.sortStates = result.sortStates;
    this.currentSort = this.queryParams.sort;
    this.getAllCancellation();
  }


  openShowDialog(id: number) {
    const cancellation = this.cancellations.find(x => x.id === id);

    const dialogRef = this.dialogService.open(ShowCancellationComponent, {
      context: {...cancellation},
    });

    dialogRef.componentRef.instance.onShowDelete.subscribe(obj => {
      this.openConfirmDialog(obj.id, obj.ticketCode);
    });
  }

  openConfirmDialog(id: number, ticketCode: string) {
    const dialogRef = this.dialogService.open(ConfirmDeleteCancellationComponent, {
      context: {
        id,
        ticketCode,
      },
    });

    dialogRef.componentRef.instance.onConfirmDelete.subscribe(_ => {
      this.getAllCancellation();
    });
  }

  onPageChanged(newPage: number) {
    this.queryParams.pageNumber = newPage;
    this.getAllCancellation();
  }

  onPageSizeChanged(newSize: number) {
    this.queryParams.pageSize = newSize;
    this.getAllCancellation();
  }
}
