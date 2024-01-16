import { Component } from '@angular/core';
import { Schedule } from '../../../../@models/schedule';
import { ScheduleService } from '../schedule.service';
import { NbDialogService } from '@nebular/theme';
import { PaginatedResult } from '../../../../@models/paginatedResult';
import { Pagination } from '../../../../@models/pagination';
import { QueryParams } from '../../../../@models/params/queryParams';
import { ConfirmDeleteScheduleComponent } from '../confirm-delete-schedule/confirm-delete-schedule.component';
import { ShowScheduleComponent } from '../show-schedule/show-schedule.component';

@Component({
  selector: 'ngx-list-schedule',
  templateUrl: './list-schedule.component.html',
  styleUrls: ['./list-schedule.component.scss']
})
export class ListScheduleComponent {
  schedules: Schedule [] = [];
  pagination: Pagination;

  currentSearchTerm = '';
  currentSort = '';

  sortStates = {
    name : false,
    trainName : false,
    departureStation: false,
    arrivalStation: false,
    departureTime: false,
    createdAt : false,
  };

  queryParams: QueryParams = {
    pageNumber: 1,
    pageSize: 10,
    searchTerm: '',
    sort: '',
  };

  constructor(private scheduleService: ScheduleService,
              private dialogService: NbDialogService) {
  }

  ngOnInit(): void {
    this.getAllSchedule();
  }

  getAllSchedule() {
    this.scheduleService.getAllSchedule(this.queryParams).subscribe({
      next: (res: PaginatedResult<Schedule[]>) => {
        this.schedules = res.result;
        this.pagination = res.pagination;

        this.checkItemsAndAdjustPage();
      },
    });
  }

  checkItemsAndAdjustPage() {
    if (this.schedules.length === 0 && this.pagination.currentPage > 1) {
      this.queryParams.pageNumber = this.pagination.currentPage - 1;

      this.getAllSchedule();
    }
  }

  onSearch() {
    this.queryParams.searchTerm = this.currentSearchTerm;
    this.queryParams.pageNumber = 1;
    this.getAllSchedule();
  }

  onResetSearch() {
    this.currentSearchTerm = '';
    this.queryParams.searchTerm = '';
    this.getAllSchedule();
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
    this.getAllSchedule();
  }

  openShowDialog(id: number) {
    const Schedule = this.schedules.find(x => x.id === id);

    const dialogRef = this.dialogService.open(ShowScheduleComponent, {
      context: {...Schedule},
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
      this.getAllSchedule();
    });
  }

  onPageChanged(newPage: number) {
    this.queryParams.pageNumber = newPage;
    this.getAllSchedule();
  }

  onPageSizeChanged(newSize: number) {
    this.queryParams.pageSize = newSize;
    this.getAllSchedule();
  }
}
