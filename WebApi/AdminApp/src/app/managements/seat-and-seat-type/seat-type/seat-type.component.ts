import {Component, OnInit} from '@angular/core';
import {SeatType} from '../../../@models/seatType';
import {PaginatedResult} from '../../../@models/paginatedResult';
import {SeatTypeService} from './seat-type.service';
import {NbDialogService} from '@nebular/theme';
import {QueryParams} from '../../../@models/params/queryParams';
import {Pagination} from '../../../@models/pagination';
import {ShowSeatTypeComponent} from './show-seat-type/show-seat-type.component';
import {ConfirmDeleteSeatTypeComponent} from './confirm-delete-seat-type/confirm-delete-seat-type.component';
import {SharedService} from '../../shared/shared.service';

@Component({
  selector: 'ngx-seat-type',
  templateUrl: './seat-type.component.html',
  styleUrls: ['./seat-type.component.scss'],
})
export class SeatTypeComponent implements OnInit {
  seatTypes: SeatType[] = [];
  pagination: Pagination;

  currentSearchTerm: string = '';
  currentSort: string = '';

  sortStates: { [key: string]: boolean } = {
    name: false,
    serviceCharge: false,
    createdAt: false,
  };

  queryParams: QueryParams = {
    pageNumber: 1,
    pageSize: 10,
    searchTerm: '',
    sort: '',
  };

  constructor(private seatTypeService: SeatTypeService,
              private sharedService: SharedService,
              private dialogService: NbDialogService) {
  }

  ngOnInit(): void {
    this.getAllSeatType();
  }

  getAllSeatType() {
    this.seatTypeService.getAllSeatType(this.queryParams).subscribe({
      next: (res: PaginatedResult<SeatType[]>) => {
        this.seatTypes = res.result;
        this.pagination = res.pagination;

        this.checkItemsAndAdjustPage();
      },
    });
  }

  checkItemsAndAdjustPage() {
    if (this.seatTypes.length === 0 && this.pagination.currentPage > 1) {
      this.queryParams.pageNumber = this.pagination.currentPage - 1;
      this.getAllSeatType();
    }
  }

  onSearch() {
    this.queryParams.searchTerm = this.currentSearchTerm;
    this.getAllSeatType();
  }

  onResetSearch() {
    this.currentSearchTerm = '';
    this.queryParams.searchTerm = '';
    this.getAllSeatType();
  }

  onSort(sort: string) {
    const result = this.sharedService.sortItems(this.queryParams, sort, this.sortStates);
    this.queryParams = result.queryParams;
    this.sortStates = result.sortStates;
    this.currentSort = this.queryParams.sort;
    this.getAllSeatType();
  }

  openShowDialog(id: number) {

    const seatType = this.seatTypes.find(x => x.id === id);

    const dialogRef = this.dialogService.open(ShowSeatTypeComponent, {
      context: { ...seatType },
    });

    dialogRef.componentRef.instance.onShowDelete.subscribe(obj => {
      this.openConfirmDialog(obj.id, obj.name);
    });

  }

  openConfirmDialog(id: number, name: string) {
    const dialogRef = this.dialogService.open(ConfirmDeleteSeatTypeComponent, {
      context: {id, name},
    });

    dialogRef.componentRef.instance.onConfirmDelete.subscribe(_ => {
      this.getAllSeatType();
    });
  }

  onPageChanged(newPage: number) {
    this.queryParams.pageNumber = newPage;
    this.getAllSeatType();
  }

  onPageSizeChanged(newSize: number) {
    this.queryParams.pageSize = newSize;
    this.getAllSeatType();
  }
}
