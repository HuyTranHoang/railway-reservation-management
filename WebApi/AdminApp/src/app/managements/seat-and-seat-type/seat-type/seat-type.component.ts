import { Component } from '@angular/core';
import { SeatType } from '../../../@models/seatType';
import { ShowCarriageTypeComponent } from '../../train-and-carriage/carriage-type/show-carriage-type/show-carriage-type.component';
import { PaginatedResult } from '../../../@models/paginatedResult';
import { SeatTypeService } from './seat-type.service';
import { NbDialogService, NbGlobalPhysicalPosition, NbToastrService } from '@nebular/theme';
import { QueryParams } from '../../../@models/params/queryParams';
import { Pagination } from '../../../@models/pagination';
import { ToastrComponent } from '../../../pages/modal-overlays/toastr/toastr.component';
import { ShowSeatTypeComponent } from './show-seat-type/show-seat-type.component';
import { ConfirmDeleteSeatTypeComponent } from './confirm-delete-seat-type/confirm-delete-seat-type.component';

@Component({
  selector: 'ngx-seat-type',
  templateUrl: './seat-type.component.html',
  styleUrls: ['./seat-type.component.scss']
})
export class SeatTypeComponent {
  seatTypes: SeatType[] = [];
  pagination: Pagination;

  currentSearchTerm: string = '';
  currentSort: string = '';
  deleteId? : number;

  // Trong component của bạn
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

  constructor(private seatTypeService: SeatTypeService, private dialogService: NbDialogService,private toasterService : NbToastrService) {
  }

  ngOnInit(): void {
    this.getAllSeatType();
  }
  getAllSeatType() {
    this.seatTypeService.getAllSeatType(this.queryParams).subscribe({
      next: (res: PaginatedResult<SeatType[]>) => {
        this.seatTypes = res.result;
        this.pagination = res.pagination;
      },
      error: (err) => {
      },
    });
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

    this.getAllSeatType();
  }
  openShowDialog(id: number) {
    this.seatTypeService.getSeatTypeById(id).subscribe({
      next: (res: SeatType) => {
        const dialogRef = this.dialogService.open(ShowSeatTypeComponent, {
          context: {
            id: res.id,
            name: res.name,
            serviceCharge: res.serviceCharge,
            description: res.description,
            status: res.status,
            createdAt: res.createdAt,
          },
        });

        dialogRef.componentRef.instance.onShowDelete.subscribe(obj => {
          this.openConfirmDialog(obj.id, obj.name);
        });
      },
    });

  }

  openConfirmDialog(id: number, name: string) {
    const dialogRef = this.dialogService.open(ConfirmDeleteSeatTypeComponent, {
      context: {
        id,
        name,
      },
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
