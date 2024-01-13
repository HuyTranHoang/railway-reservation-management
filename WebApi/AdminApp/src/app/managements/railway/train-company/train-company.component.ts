import {Component, OnInit} from '@angular/core';
import {TrainCompanyService} from './train-company.service';
import {TrainCompany} from '../../../@models/trainCompany';
import {Pagination} from '../../../@models/pagination';
import {PaginatedResult} from '../../../@models/paginatedResult';
import {QueryParams} from '../../../@models/params/queryParams';
import {ShowTrainCompanyComponent} from './show-train-company/show-train-company.component';
import {
  ConfirmDeleteTrainCompanyComponent,
} from './confirm-delete-train-company/confirm-delete-train-company.component';
import {NbDialogService} from '@nebular/theme';

@Component({
  selector: 'ngx-train-company',
  templateUrl: './train-company.component.html',
  styleUrls: ['./train-company.component.scss'],
})
export class TrainCompanyComponent implements OnInit {

  trainCompanies: TrainCompany[] = [];
  pagination: Pagination;

  currentSearchTerm: string = '';
  currentSort: string = '';

  queryParams: QueryParams = {
    pageNumber: 1,
    pageSize: 5,
    searchTerm: '',
    sort: '',
  };

  sortStates: { [key: string]: boolean } = {
    name: false,
    createdAt: false,
  };

  constructor(private trainCompanyService: TrainCompanyService, private dialogService: NbDialogService) {
  }

  ngOnInit(): void {
    this.getAllTrainCompany();
  }

  getAllTrainCompany() {
    this.trainCompanyService.getAllTrainCompany(this.queryParams).subscribe({
      next: (res: PaginatedResult<TrainCompany[]>) => {
        this.trainCompanies = res.result;
        this.pagination = res.pagination;

        this.checkItemsAndAdjustPage();
      },
    });
  }

  checkItemsAndAdjustPage() {
    if (this.trainCompanies.length === 0 && this.pagination.currentPage > 1) {
      this.queryParams.pageNumber = this.pagination.currentPage - 1;
      this.getAllTrainCompany();
    }
  }

  onSearch() {
    this.queryParams.searchTerm = this.currentSearchTerm;
    this.queryParams.pageNumber = 1;
    this.getAllTrainCompany();
  }

  onResetSearch() {
    this.currentSearchTerm = '';
    this.queryParams.searchTerm = '';
    this.getAllTrainCompany();
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
    this.getAllTrainCompany();
  }


  openShowDialog(id: number) {
    const trainCompany = this.trainCompanies.find(x => x.id === id);

    const dialogRef = this.dialogService.open(ShowTrainCompanyComponent, {
      context: { ...trainCompany },
    });

    dialogRef.componentRef.instance.onShowDelete.subscribe(obj => {
      this.openConfirmDialog(obj.id, obj.name);
    });
  }

  openConfirmDialog(id: number, name: string) {
    const dialogRef = this.dialogService.open(ConfirmDeleteTrainCompanyComponent, {
      context: {
        id,
        name,
      },
    });

    dialogRef.componentRef.instance.onConfirmDelete.subscribe(_ => {
      this.getAllTrainCompany();
    });
  }

  onPageChanged(newPage: number) {
    this.queryParams.pageNumber = newPage;
    this.getAllTrainCompany();
  }

  onPageSizeChanged(newSize: number) {
    this.queryParams.pageSize = newSize;
    this.getAllTrainCompany();
  }
}
