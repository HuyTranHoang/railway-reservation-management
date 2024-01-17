import {Train} from '../../../../@models/train';
import {Component, OnInit} from '@angular/core';
import {NbDialogService} from '@nebular/theme';
import {PaginatedResult} from '../../../../@models/paginatedResult';
import {Pagination} from '../../../../@models/pagination';
import {TrainService} from '../train.service';
import {ConfirmDeleteTrainComponent} from '../confirm-delete-train/confirm-delete-train.component';
import {ShowTrainComponent} from '../show-train/show-train.component';
import {TrainQueryParams} from '../../../../@models/params/trainQueryParams';
import {SharedService} from '../../../shared/shared.service';
import {TrainCompanyService} from '../../../railway/train-company/train-company.service';

@Component({
  selector: 'ngx-list-train',
  templateUrl: './list-train.component.html',
  styleUrls: ['./list-train.component.scss'],
})
export class ListTrainComponent implements OnInit {
  trains: Train [] = [];
  pagination: Pagination;

  trainCompaniesFilter: { id: number, name: string }[] = [];

  currentSearchTerm = '';
  currentSort = '';

  sortStates = {
    name: false,
    trainCompanyName: false,
    status: false,
    createdAt: false,
  };

  queryParams: TrainQueryParams = {
    pageNumber: 1,
    pageSize: 10,
    searchTerm: '',
    sort: '',
    trainCompanyId: null,
  };

  constructor(private trainService: TrainService,
              private trainCompanyService: TrainCompanyService,
              private sharedService: SharedService,
              private dialogService: NbDialogService) {
  }

  ngOnInit(): void {
    this.loadAllTrain();
    this.loadTrainCompany();
  }

  loadAllTrain() {
    this.trainService.getAllTrain(this.queryParams).subscribe({
      next: (res: PaginatedResult<Train[]>) => {
        this.trains = res.result;
        this.pagination = res.pagination;

        this.checkItemsAndAdjustPage();
      },
    });
  }

  loadTrainCompany() {
    this.trainCompanyService.getAllTrainCompanyNoPaging().subscribe({
      next: (res: Train[]) => {
        this.trainCompaniesFilter = [{id: 0, name: 'All train companies'} , ...res];
      },
    });
  }

  checkItemsAndAdjustPage() {
    if (this.trains.length === 0 && this.pagination.currentPage > 1) {
      this.queryParams.pageNumber = this.pagination.currentPage - 1;

      this.loadAllTrain();
    }
  }

  onSearch() {
    this.queryParams.searchTerm = this.currentSearchTerm;
    this.queryParams.pageNumber = 1;
    this.loadAllTrain();
  }

  onResetSearch() {
    this.currentSearchTerm = '';
    this.queryParams.searchTerm = '';
    this.loadAllTrain();
  }

  onSort(sort: string) {
    const result = this.sharedService.sortItems(this.queryParams, sort, this.sortStates);
    this.queryParams = result.queryParams;
    this.sortStates = result.sortStates;
    this.currentSort = this.queryParams.sort;
    this.loadAllTrain();
  }

  onFilterTrainCompany(trainCompanyId: number) {
    this.loadAllTrain();
  }

  onFilterReset() {
    this.queryParams.trainCompanyId = null;
    this.loadAllTrain();
  }

  openShowDialog(id: number) {
    const train = this.trains.find(x => x.id === id);

    const dialogRef = this.dialogService.open(ShowTrainComponent, {
      context: {...train},
    });

    dialogRef.componentRef.instance.onShowDelete.subscribe(obj => {
      this.openConfirmDialog(obj.id, obj.name);
    });

  }

  openConfirmDialog(id: number, name: string) {
    const dialogRef = this.dialogService.open(ConfirmDeleteTrainComponent, {
      context: {id, name},
    });

    dialogRef.componentRef.instance.onConfirmDelete.subscribe((_: any) => {
      this.loadAllTrain();
    });
  }

  onPageChanged(newPage: number) {
    this.queryParams.pageNumber = newPage;
    this.loadAllTrain();
  }

  onPageSizeChanged(newSize: number) {
    this.queryParams.pageSize = newSize;
    this.loadAllTrain();
  }

}
