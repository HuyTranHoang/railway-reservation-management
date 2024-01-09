import {Component, OnInit} from '@angular/core';
import {CarriageTypeService} from './carriage-type.service';
import {CarriageType} from '../../../@models/carriageType';
import {Pagination} from '../../../@models/pagination';
import {PaginatedResult} from '../../../@models/paginatedResult';
import {QueryParams} from '../../../@models/params/queryParams';
import {NbDialogService} from '@nebular/theme';
import {ShowCarriageTypeComponent} from './show-carriage-type/show-carriage-type.component';
import {
  ConfirmDeleteCarriageTypeComponent,
} from './confirm-delete-carriage-type/confirm-delete-carriage-type.component';

@Component({
  selector: 'ngx-carriage-type',
  templateUrl: './carriage-type.component.html',
  styleUrls: ['./carriage-type.component.scss'],
})
export class CarriageTypeComponent implements OnInit {

  carriageTypes: CarriageType[] = [];
  pagination: Pagination;

  currentSearchTerm: string = '';
  currentSort: string = '';

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

  constructor(private carriageTypeService: CarriageTypeService, private dialogService: NbDialogService) {
  }

  ngOnInit(): void {
    this.getAllCarriageType();
  }

  getAllCarriageType() {
    this.carriageTypeService.getAllCarriageType(this.queryParams).subscribe({
      next: (res: PaginatedResult<CarriageType[]>) => {
        this.carriageTypes = res.result;
        this.pagination = res.pagination;
      },
    });
  }

  onSearch() {
    this.queryParams.searchTerm = this.currentSearchTerm;
    this.getAllCarriageType();
  }

  onResetSearch() {
    this.currentSearchTerm = '';
    this.queryParams.searchTerm = '';
    this.getAllCarriageType();
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

    this.getAllCarriageType();
  }


  openShowDialog(id: number) {
    this.carriageTypeService.getCarriageTypeById(id).subscribe({
      next: (res: CarriageType) => {
        const dialogRef = this.dialogService.open(ShowCarriageTypeComponent, {
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
    const dialogRef = this.dialogService.open(ConfirmDeleteCarriageTypeComponent, {
      context: {
        id,
        name,
      },
    });

    dialogRef.componentRef.instance.onConfirmDelete.subscribe(_ => {
      this.getAllCarriageType();
    });
  }

}
