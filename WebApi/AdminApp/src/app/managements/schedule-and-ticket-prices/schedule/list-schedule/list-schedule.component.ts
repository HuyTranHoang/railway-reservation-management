import {Component, OnInit} from '@angular/core';
import {Schedule} from '../../../../@models/schedule';
import {ScheduleService} from '../schedule.service';
import {NbDialogService} from '@nebular/theme';
import {PaginatedResult} from '../../../../@models/paginatedResult';
import {Pagination} from '../../../../@models/pagination';
import {QueryParams} from '../../../../@models/params/queryParams';
import {ConfirmDeleteScheduleComponent} from '../confirm-delete-schedule/confirm-delete-schedule.component';
import {ShowScheduleComponent} from '../show-schedule/show-schedule.component';
import {ScheduleQueryParams} from '../../../../@models/params/scheduleQueryParams';
import {SharedService} from '../../../shared/shared.service';
import {TrainStationService} from '../../../railway/train-station/train-station.service';
import {TrainStation} from '../../../../@models/trainStation';

@Component({
  selector: 'ngx-list-schedule',
  templateUrl: './list-schedule.component.html',
  styleUrls: ['./list-schedule.component.scss'],
})
export class ListScheduleComponent implements OnInit {
  schedules: Schedule [] = [];
  pagination: Pagination;

  currentSearchTerm = '';
  currentSort = '';

  departureStationFilter: { id: number, name: string }[] = [];

  sortStates = {
    scheduleName: false,
    trainName: false,
    departureStationName: false,
    arrivalStationName: false,
    departureTime: false,
    createdAt: false,
  };

  queryParams: ScheduleQueryParams = {
    pageNumber: 1,
    pageSize: 10,
    searchTerm: '',
    sort: '',
    trainId: null,
    departureStationId: null,
    arrivalStationId: null,
  };

  constructor(private scheduleService: ScheduleService,
              private trainStationService: TrainStationService,
              private sharedService: SharedService,
              private dialogService: NbDialogService) {
  }

  ngOnInit(): void {
    this.loadAllTrainStations();
    this.loadAllSchedule();
  }

  loadAllSchedule() {
    this.scheduleService.getAllSchedule(this.queryParams).subscribe({
      next: (res: PaginatedResult<Schedule[]>) => {
        this.schedules = res.result;
        this.pagination = res.pagination;

        this.checkItemsAndAdjustPage();
      },
    });
  }

  loadAllTrainStations() {
    this.trainStationService.getAllTrainStationNoPaging().subscribe({
      next: (res: TrainStation[]) => {
        this.departureStationFilter = [{id: 0, name: 'All departure station'} , ...res];
      },
    });
  }

  checkItemsAndAdjustPage() {
    if (this.schedules.length === 0 && this.pagination.currentPage > 1) {
      this.queryParams.pageNumber = this.pagination.currentPage - 1;

      this.loadAllSchedule();
    }
  }

  onSearch() {
    this.queryParams.searchTerm = this.currentSearchTerm;
    this.queryParams.pageNumber = 1;
    this.loadAllSchedule();
  }

  onResetSearch() {
    this.currentSearchTerm = '';
    this.queryParams.searchTerm = '';
    this.loadAllSchedule();
  }

  onSort(sort: string) {
    const result = this.sharedService.sortItems(this.queryParams, sort, this.sortStates);
    this.queryParams = result.queryParams;
    this.sortStates = result.sortStates;
    this.currentSort = this.queryParams.sort;
    this.loadAllSchedule();
  }

  openShowDialog(id: number) {
    const schedule = this.schedules.find(x => x.id === id);

    const dialogRef = this.dialogService.open(ShowScheduleComponent, {
      context: {...schedule},
    });

    dialogRef.componentRef.instance.onShowDelete.subscribe(obj => {
      this.openConfirmDialog(obj.id, obj.name);
    });

  }

  openConfirmDialog(id: number, name: string) {
    const dialogRef = this.dialogService.open(ConfirmDeleteScheduleComponent, {
      context: {id, name},
    });

    dialogRef.componentRef.instance.onConfirmDelete.subscribe((_: any) => {
      this.loadAllSchedule();
    });
  }

  onPageChanged(newPage: number) {
    this.queryParams.pageNumber = newPage;
    this.loadAllSchedule();
  }

  onPageSizeChanged(newSize: number) {
    this.queryParams.pageSize = newSize;
    this.loadAllSchedule();
  }

  onFilterDepartureStation(trainStationId: number) {
    this.loadAllSchedule();
  }

  onFilterReset() {
    this.queryParams.trainId = null;
    this.queryParams.departureStationId = null;
    this.queryParams.arrivalStationId = null;
    this.loadAllSchedule();
  }
}
