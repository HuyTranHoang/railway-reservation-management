import {Component, OnInit} from '@angular/core';
import {SeatService} from '../seat.service';
import {NbDialogService} from '@nebular/theme';
import {PaginatedResult} from '../../../../@models/paginatedResult';
import {Pagination} from '../../../../@models/pagination';
import {QueryParams} from '../../../../@models/params/queryParams';
import {Seat} from '../../../../@models/seat';
import {ConfirmDeleteSeatComponent} from '../confirm-delete-seat/confirm-delete-seat.component';
import {ShowSeatComponent} from '../show-seat/show-seat.component';

@Component({
  selector: 'ngx-list-seat',
  templateUrl: './list-seat.component.html',
  styleUrls: ['./list-seat.component.scss']
})
export class ListSeatComponent implements OnInit{
  seats: Seat [] = [];
  pagination: Pagination;

  currentSearchTerm = '';
  currentSort = '';

  sortStates = {
    name: false,
    seatTypeName : false,
    compartmentName: false,
    status: false,
    createdAt: false,
  };

  queryParams: QueryParams = {
    pageNumber: 1,
    pageSize: 10,
    searchTerm: '',
    sort: '',
  };

  constructor(private seatService: SeatService,
              private dialogService: NbDialogService) {
  }

  ngOnInit(): void {
    this.getAllSeat();
  }

  getAllSeat() {
    this.seatService.getAllSeat(this.queryParams).subscribe({
      next: (res: PaginatedResult<Seat[]>) => {
        this.seats = res.result;
        this.pagination = res.pagination;

        this.checkItemsAndAdjustPage();
      },
    });
  }

  checkItemsAndAdjustPage() {
    if (this.seats.length === 0 && this.pagination.currentPage > 1) {
      this.queryParams.pageNumber = this.pagination.currentPage - 1;

      this.getAllSeat();
    }
  }

  onSearch() {
    this.queryParams.searchTerm = this.currentSearchTerm;
    this.queryParams.pageNumber = 1;
    this.getAllSeat();
  }

  onResetSearch() {
    this.currentSearchTerm = '';
    this.queryParams.searchTerm = '';
    this.getAllSeat();
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
    this.getAllSeat();
  }

  openShowDialog(id: number) {
    const Seat = this.seats.find(x => x.id === id);

    const dialogRef = this.dialogService.open(ShowSeatComponent, {
      context: {...Seat},
    });

    dialogRef.componentRef.instance.onShowDelete.subscribe(obj => {
      this.openConfirmDialog(obj.id, obj.name);
    });

  }

  openConfirmDialog(id: number, name: string) {
    const dialogRef = this.dialogService.open(ConfirmDeleteSeatComponent, {
      context: {id, name},
    });

    dialogRef.componentRef.instance.ConfirmDelete.subscribe((_: any) => {
      this.getAllSeat();
    });
  }

  onPageChanged(newPage: number) {
    this.queryParams.pageNumber = newPage;
    this.getAllSeat();
  }

  onPageSizeChanged(newSize: number) {
    this.queryParams.pageSize = newSize;
    this.getAllSeat();
  }
}
