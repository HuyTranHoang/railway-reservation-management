import {Component, OnInit} from '@angular/core';
import {RoundTrip} from '../../../../@models/roundTrip';
import {Pagination} from '../../../../@models/pagination';
import {QueryParams} from '../../../../@models/params/queryParams';
import {RoundTripService} from '../round-trip.service';
import {NbDialogService} from '@nebular/theme';
import {PaginationService} from '../../../shared/pagination.service';
import {PaginatedResult} from '../../../../@models/paginatedResult';
import {ShowRoundTripComponent} from '../show-round-trip/show-round-trip.component';
import {ConfirmDeleteRoundTripComponent} from '../confirm-delete-round-trip/confirm-delete-round-trip.component';

@Component({
  selector: 'ngx-list-round-trip',
  templateUrl: './list-round-trip.component.html',
  styleUrls: ['./list-round-trip.component.scss'],
})
export class ListRoundTripComponent implements OnInit {

  roundTrips: RoundTrip [] = [];
  pagination: Pagination;

  currentSearchTerm = '';
  currentSort = '';

  sortStates = {
    name: false,
    trainCompanyName: false,
    discount: false,
    createdAt: false,
  };

  queryParams: QueryParams = {
    pageNumber: 1,
    pageSize: 10,
    searchTerm: '',
    sort: '',
  };

  constructor(private roundTripService: RoundTripService,
              private dialogService: NbDialogService,
              private paginationService: PaginationService) {
  }

  ngOnInit(): void {
    this.getAllRoundTrip();
  }

  getAllRoundTrip() {
    this.roundTripService.getAllRoundTrip(this.queryParams).subscribe({
      next: (res: PaginatedResult<RoundTrip[]>) => {
        this.roundTrips = res.result;
        this.pagination = res.pagination;

        this.checkItemsAndAdjustPage();
      },
    });
  }

  checkItemsAndAdjustPage() {
    if (this.roundTrips.length === 0 && this.pagination.currentPage > 1) {
      this.queryParams.pageNumber = this.pagination.currentPage - 1;
      this.getAllRoundTrip();
    }
  }

  onSearch() {
    this.queryParams.searchTerm = this.currentSearchTerm;
    this.queryParams.pageNumber = 1;
    this.getAllRoundTrip();
  }

  onResetSearch() {
    this.currentSearchTerm = '';
    this.queryParams.searchTerm = '';
    this.getAllRoundTrip();
  }

  onSort(sort: string) {
    const sortType = sort.split('Asc')[0];

    if (this.queryParams.sort === sort) {
      this.queryParams.sort = sort.endsWith('Asc') ? sort.replace('Asc', 'Desc') : sort.replace('Desc', 'Asc');
      this.sortStates[sortType] = !this.sortStates[sortType];
    } else {
      this.queryParams.sort = sort;
      for (const key in this.sortStates) {
        if (this.sortStates.hasOwnProperty(key)) {
          this.sortStates[key] = false;
        }
      }

      this.sortStates[sortType] = sort.endsWith('Asc');
    }

    this.currentSort = this.queryParams.sort;

    this.queryParams.pageNumber = 1;
    this.getAllRoundTrip();
  }

  openShowDialog(id: number) {
    const roundTrip = this.roundTrips.find(x => x.id === id);

    const dialogRef = this.dialogService.open(ShowRoundTripComponent, {
      context: { ...roundTrip },
    });

    dialogRef.componentRef.instance.onShowDelete.subscribe(obj => {
      this.openConfirmDialog(obj.id, obj.name);
    });
  }

  openConfirmDialog(id: number, name: string) {
    const dialogRef = this.dialogService.open(ConfirmDeleteRoundTripComponent, {
      context: { id, name },
    });

    dialogRef.componentRef.instance.onConfirmDelete.subscribe((_: any) => {
      this.getAllRoundTrip();
    });
  }

  onPageChanged(newPage: number) {
    this.queryParams.pageNumber = newPage;
    this.getAllRoundTrip();
  }

  onPageSizeChanged(newSize: number) {
    this.queryParams.pageSize = newSize;
    this.getAllRoundTrip();
  }
}
