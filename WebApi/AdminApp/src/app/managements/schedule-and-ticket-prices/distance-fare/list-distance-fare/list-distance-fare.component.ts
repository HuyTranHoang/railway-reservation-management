import {Component, OnInit} from '@angular/core';
import {DistanceFare} from '../../../../@models/distanceFare';
import {Pagination} from '../../../../@models/pagination';
import {QueryParams} from '../../../../@models/params/queryParams';
import {DistanceFareService} from '../distance-fare.service';
import {NbDialogService} from '@nebular/theme';
import {PaginatedResult} from '../../../../@models/paginatedResult';
import {ShowDistanceFareComponent} from '../show-distance-fare/show-distance-fare.component';
import {
  ConfirmDeleteDistanceFareComponent,
} from '../confirm-delete-distance-fare/confirm-delete-distance-fare.component';
import {SharedService} from '../../../shared/shared.service';

@Component({
  selector: 'ngx-list-distance-fare',
  templateUrl: './list-distance-fare.component.html',
  styleUrls: ['./list-distance-fare.component.scss'],
})
export class ListDistanceFareComponent implements OnInit {
  distanceFares: DistanceFare[] = [];
  pagination: Pagination;

  currentSearchTerm: string = '';
  currentSort: string = '';

  sortStates = {
    trainCompanyName: false,
    distance: false,
    price: false,
    createdAt: false,
  };

  queryParams: QueryParams = {
    pageNumber: 1,
    pageSize: 10,
    searchTerm: '',
    sort: '',
  };

  constructor(private distanceFareService: DistanceFareService,
              private sharedService: SharedService,
              private dialogService: NbDialogService) {
  }

  ngOnInit(): void {
    this.getAllDistanceFare();
  }

  getAllDistanceFare() {
    this.distanceFareService.getAllDistanceFares(this.queryParams).subscribe({
      next: (res: PaginatedResult<DistanceFare[]>) => {
        this.distanceFares = res.result;
        this.pagination = res.pagination;

        this.checkItemsAndAdjustPage();
      },
    });
  }

  checkItemsAndAdjustPage() {
    if (this.distanceFares.length === 0 && this.pagination.currentPage > 1) {
      this.queryParams.pageNumber = this.pagination.currentPage - 1;
      this.getAllDistanceFare();
    }
  }

  onSearch() {
    this.queryParams.searchTerm = this.currentSearchTerm;
    this.getAllDistanceFare();
  }

  onResetSearch() {
    this.currentSearchTerm = '';
    this.queryParams.searchTerm = '';
    this.getAllDistanceFare();
  }

  onSort(sort: string) {
    const result = this.sharedService.sortItems(this.queryParams, sort, this.sortStates);
    this.queryParams = result.queryParams;
    this.sortStates = result.sortStates;
    this.currentSort = this.queryParams.sort;
    this.getAllDistanceFare();
  }

  openShowDialog(id: number) {

    const distanceFare = this.distanceFares.find(x => x.id === id);

    const dialogRef = this.dialogService.open(ShowDistanceFareComponent, {
      context: { ...distanceFare },
    });

    dialogRef.componentRef.instance.onShowDelete.subscribe(obj => {
      this.openConfirmDialog(obj.id, obj.trainCompanyName);
    });

  }

  openConfirmDialog(id: number, trainCompanyName: string) {
    const dialogRef = this.dialogService.open(ConfirmDeleteDistanceFareComponent, {
      context: { id, trainCompanyName },
    });

    dialogRef.componentRef.instance.onConfirmDelete.subscribe(_ => {
      this.getAllDistanceFare();
    });
  }

  onPageChanged(newPage: number) {
    this.queryParams.pageNumber = newPage;
    this.getAllDistanceFare();
  }

  onPageSizeChanged(newSize: number) {
    this.queryParams.pageSize = newSize;
    this.getAllDistanceFare();
  }
}
