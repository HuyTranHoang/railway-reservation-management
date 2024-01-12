import {Train} from '../../../../@models/train';
import {Component, OnInit} from '@angular/core';
import {NbDialogService} from '@nebular/theme';
import {PaginatedResult} from '../../../../@models/paginatedResult';
import {Pagination} from '../../../../@models/pagination';
import {QueryParams} from '../../../../@models/params/queryParams';
import {TrainService} from '../train.service';
import {ConfirmDeleteTrainComponent} from '../confirm-delete-train/confirm-delete-train.component';

@Component({
  selector: 'ngx-list-train',
  templateUrl: './list-train.component.html',
  styleUrls: ['./list-train.component.scss'],
})
export class ListTrainComponent implements OnInit {
  trains: Train [] = [];
  pagination: Pagination;

  currentSearchTerm = '';
  currentSort = '';

  sortStates = {
    name: false,
    trainCompanyName: false,
    createdAt: false,
  };


  queryParams: QueryParams = {
    pageNumber: 1,
    pageSize: 5,
    searchTerm: '',
    sort: '',
  };

  constructor(private trainService: TrainService,
              private dialogService: NbDialogService) {
  }

  ngOnInit(): void {
    this.getAllTrain();
  }

  getAllTrain() {
    this.trainService.getAllTrain(this.queryParams).subscribe({
      next: (res: PaginatedResult<Train[]>) => {
        this.trains = res.result;
        this.pagination = res.pagination;

        this.checkItemsAndAdjustPage();
      },
    });
  }

  checkItemsAndAdjustPage() {
    if (this.trains.length === 0 && this.pagination.currentPage > 1) {
      this.queryParams.pageNumber = this.pagination.currentPage - 1;

      this.getAllTrain();
    }
  }

  onSearch() {
    this.queryParams.searchTerm = this.currentSearchTerm;
    this.queryParams.pageNumber = 1;
    this.getAllTrain();
  }

  onResetSearch() {
    this.currentSearchTerm = '';
    this.queryParams.searchTerm = '';
    this.getAllTrain();
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
    this.getAllTrain();
  }

  openConfirmDialog(id: number, name: string) {
    const dialogRef = this.dialogService.open(ConfirmDeleteTrainComponent, {
      context: {id, name},
    });

    dialogRef.componentRef.instance.onConfirmDelete.subscribe((_: any) => {
      this.getAllTrain();
    });
  }

  onPageChanged(newPage: number) {
    this.queryParams.pageNumber = newPage;
    this.getAllTrain();
  }

  onPageSizeChanged(newSize: number) {
    this.queryParams.pageSize = newSize;
    this.getAllTrain();
  }

}
