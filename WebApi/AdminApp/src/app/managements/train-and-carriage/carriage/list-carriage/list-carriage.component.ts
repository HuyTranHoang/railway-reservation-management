import {Component, OnInit} from '@angular/core';
import {Pagination} from '../../../../@models/pagination';
import {NbDialogService} from '@nebular/theme';
import {PaginatedResult} from '../../../../@models/paginatedResult';
import {
  ConfirmDeleteCarriageTypeComponent,
} from '../../carriage-type/confirm-delete-carriage-type/confirm-delete-carriage-type.component';
import {Carriage} from '../../../../@models/carriage';
import {CarriageService} from '../carriage.service';
import {CarriageQueryParams} from '../../../../@models/params/carriageQueryParams';
import {ShowCarriageComponent} from '../show-carriage/show-carriage.component';
import {SharedService} from '../../../shared/shared.service';
import {CarriageTypeService} from '../../carriage-type/carriage-type.service';
import {CarriageType} from '../../../../@models/carriageType';

@Component({
  selector: 'ngx-list-carriage',
  templateUrl: './list-carriage.component.html',
  styleUrls: ['./list-carriage.component.scss'],
})
export class ListCarriageComponent implements OnInit {

  carriages: Carriage[] = [];
  pagination: Pagination;

  carriageTypesFilter: { id: number, name: string }[] = [];

  currentSearchTerm: string = '';
  currentSort: string = '';

  sortStates = {
    name: false,
    trainName: false,
    typeName: false,
    numberOfCompartment: false,
    createdAt: false,
  };

  queryParams: CarriageQueryParams = {
    pageNumber: 1,
    pageSize: 10,
    searchTerm: '',
    sort: '',
    trainId: null,
    carriageTypeId: null,
  };

  constructor(private carriageService: CarriageService,
              private carriageTypeService: CarriageTypeService,
              private sharedService: SharedService,
              private dialogService: NbDialogService) {
  }

  ngOnInit(): void {
    this.loadAllCarriages();
    this.loadAllCarriageTypes();
  }

  loadAllCarriages() {
    this.carriageService.getAllCarriages(this.queryParams).subscribe({
      next: (res: PaginatedResult<Carriage[]>) => {
        this.carriages = res.result;
        this.pagination = res.pagination;

        this.checkItemsAndAdjustPage();
      },
    });
  }

  loadAllCarriageTypes() {
    this.carriageTypeService.getAllCarriageTypeNoPaging().subscribe({
      next: (res: CarriageType[]) => {
        this.carriageTypesFilter = [{id: 0, name: 'All carriage type' }, ...res];
      },
    });
  }

  checkItemsAndAdjustPage() {
    if (this.carriages.length === 0 && this.pagination.currentPage > 1) {
      this.queryParams.pageNumber = this.pagination.currentPage - 1;
      this.loadAllCarriages();
    }
  }

  onSearch() {
    this.queryParams.searchTerm = this.currentSearchTerm;
    this.queryParams.pageNumber = 1;
    this.loadAllCarriages();
  }

  onResetSearch() {
    this.currentSearchTerm = '';
    this.queryParams.searchTerm = '';
    this.loadAllCarriages();
  }

  onSort(sort: string) {
    const result = this.sharedService.sortItems(this.queryParams, sort, this.sortStates);
    this.queryParams = result.queryParams;
    this.sortStates = result.sortStates;
    this.currentSort = this.queryParams.sort;
    this.loadAllCarriages();
  }

  onFilterCarriageTypes(carriageTypeId: number) {
    this.loadAllCarriages();
  }

  onFilterReset() {
    this.queryParams.trainId = null;
    this.queryParams.carriageTypeId = null;
    this.loadAllCarriages();
  }

  openShowDialog(id: number) {
    const carriage = this.carriages.find(x => x.id === id);

    const dialogRef = this.dialogService.open(ShowCarriageComponent, {
      context: {...carriage},
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
      this.loadAllCarriages();
    });
  }

  onPageChanged(newPage: number) {
    this.queryParams.pageNumber = newPage;
    this.loadAllCarriages();
  }

  onPageSizeChanged(newSize: number) {
    this.queryParams.pageSize = newSize;
    this.loadAllCarriages();
  }

}
