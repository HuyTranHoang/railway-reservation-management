import {Component, OnInit} from '@angular/core';
import {CarriageType} from '../../../../@models/carriageType';
import {Pagination} from '../../../../@models/pagination';
import {QueryParams} from '../../../../@models/params/queryParams';
import {CarriageTypeService} from '../carriage-type.service';
import {NbDialogService} from '@nebular/theme';
import {PaginatedResult} from '../../../../@models/paginatedResult';
import {ShowCarriageTypeComponent} from '../show-carriage-type/show-carriage-type.component';
import {
  ConfirmDeleteCarriageTypeComponent,
} from '../confirm-delete-carriage-type/confirm-delete-carriage-type.component';
import {SharedService} from '../../../shared/shared.service';

@Component({
  selector: 'ngx-list-carriage-type',
  templateUrl: './list-carriage-type.component.html',
  styleUrls: ['./list-carriage-type.component.scss'],
})
export class ListCarriageTypeComponent implements OnInit {
  carriageTypes: CarriageType[] = [];
  pagination: Pagination;

  currentSearchTerm: string = '';
  currentSort: string = '';

  sortStates = {
    name: false,
    numberOfCompartment: false,
    serviceCharge: false,
    createdAt: false,
  };

  queryParams: QueryParams = {
    pageNumber: 1,
    pageSize: 10,
    searchTerm: '',
    sort: '',
  };

  constructor(private carriageTypeService: CarriageTypeService,
              private sharedService: SharedService,
              private dialogService: NbDialogService) {
  }

  ngOnInit(): void {
    this.getAllCarriageType();
  }

  getAllCarriageType() {
    this.carriageTypeService.getAllCarriageType(this.queryParams).subscribe({
      next: (res: PaginatedResult<CarriageType[]>) => {
        this.carriageTypes = res.result;
        this.pagination = res.pagination;

        this.checkItemsAndAdjustPage();
      },
    });
  }

  checkItemsAndAdjustPage() {
    if (this.carriageTypes.length === 0 && this.pagination.currentPage > 1) {
      this.queryParams.pageNumber = this.pagination.currentPage - 1;
      this.getAllCarriageType();
    }
  }

  onSearch() {
    this.queryParams.searchTerm = this.currentSearchTerm;
    this.queryParams.pageNumber = 1;
    this.getAllCarriageType();
  }

  onResetSearch() {
    this.currentSearchTerm = '';
    this.queryParams.searchTerm = '';
    this.getAllCarriageType();
  }

  onSort(sort: string) {
    const result = this.sharedService.sortItems(this.queryParams, sort, this.sortStates);
    this.queryParams = result.queryParams;
    this.sortStates = result.sortStates;
    this.currentSort = this.queryParams.sort;
    this.getAllCarriageType();
  }

  openShowDialog(id: number) {
    const carriageType = this.carriageTypes.find(x => x.id === id);

    const dialogRef = this.dialogService.open(ShowCarriageTypeComponent, {
      context: {...carriageType},
    });

    dialogRef.componentRef.instance.onShowDelete.subscribe(obj => {
      this.openConfirmDialog(obj.id, obj.name);
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

  onPageChanged(newPage: number) {
    this.queryParams.pageNumber = newPage;
    this.getAllCarriageType();
  }

  onPageSizeChanged(newSize: number) {
    this.queryParams.pageSize = newSize;
    this.getAllCarriageType();
  }
}
