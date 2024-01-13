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
    pageSize: 5,
    searchTerm: '',
    sort: '',
  };

  constructor(private distanceFareService: DistanceFareService,
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
