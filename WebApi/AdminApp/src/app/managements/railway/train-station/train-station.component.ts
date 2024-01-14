import {Component, OnInit} from '@angular/core';
import {TrainStations} from '../../../@models/trainStation';
import {Pagination} from '../../../@models/pagination';
import {QueryParams} from '../../../@models/params/queryParams';
import {TrainStationService} from './train-station.service';
import {NbDialogService, NbToastrService} from '@nebular/theme';
import {PaginatedResult} from '../../../@models/paginatedResult';
import {ShowTrainStationComponent} from './show-train-station/show-train-station.component';
import {DeleteTrainStationComponent} from './delete-train-station/delete-train-station.component';

@Component({
  selector: 'ngx-train-station',
  templateUrl: './train-station.component.html',
  styleUrls: ['./train-station.component.scss'],
})
export class TrainStationComponent implements OnInit {
  trainStations: TrainStations[] = [];
  pagination: Pagination;

  currentSearchTerm: string = '';
  currentSort: string = '';

  sortStates = {
    name: false,
    address: false,
    coordinateValue: false,
    status: false,
    createdAt: false,
  };

  queryParams: QueryParams = {
    pageNumber: 1,
    pageSize: 10,
    searchTerm: '',
    sort: '',
  };

  constructor(private trainStationService: TrainStationService,
              private dialogService: NbDialogService) {
  }

  ngOnInit(): void {
    this.getAllTrainStation();
  }

  getAllTrainStation() {
    this.trainStationService.getAllTrainStation(this.queryParams).subscribe({
      next: (res: PaginatedResult<TrainStations[]>) => {
        this.trainStations = res.result;
        this.pagination = res.pagination;

        this.checkItemsAndAdjustPage();
      },
    });
  }

  checkItemsAndAdjustPage() {
    if (this.trainStations.length === 0 && this.pagination.currentPage > 1) {
      this.queryParams.pageNumber = this.pagination.currentPage - 1;
      this.getAllTrainStation();
    }
  }

  onSearch() {
    this.queryParams.searchTerm = this.currentSearchTerm;
    this.getAllTrainStation();
  }

  onResetSearch() {
    this.currentSearchTerm = '';
    this.queryParams.searchTerm = '';
    this.getAllTrainStation();
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

    this.getAllTrainStation();
  }

  openShowDialog(id: number) {
    const trainStation = this.trainStations.find(x => x.id === id);

    const dialogRef = this.dialogService.open(ShowTrainStationComponent, {
      context: { ...trainStation },
    });

    dialogRef.componentRef.instance.onShowDelete.subscribe(obj => {
      this.openConfirmDialog(obj.id, obj.name);
    });

  }

  openConfirmDialog(id: number, name: string) {
    const dialogRef = this.dialogService.open(DeleteTrainStationComponent, {
      context: { id, name },
    });

    dialogRef.componentRef.instance.onConfirmDelete.subscribe(_ => {
      this.getAllTrainStation();
    });
  }

  onPageChanged(newPage: number) {
    this.queryParams.pageNumber = newPage;
    this.getAllTrainStation();
  }

  onPageSizeChanged(newSize: number) {
    this.queryParams.pageSize = newSize;
    this.getAllTrainStation();
  }
}
