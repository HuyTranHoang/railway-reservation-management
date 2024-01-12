import { Component, OnInit } from '@angular/core';
import { RoundTripService } from './round-trip.service';
import { RoundTrip } from '../../../@models/roundTrip';
import { Pagination } from '../../../@models/pagination';
import { PaginatedResult } from '../../../@models/paginatedResult';
import { QueryParams } from '../../../@models/params/queryParams';
import { ShowRoundTripComponent } from './show-round-trip/show-round-trip.component';
import { ConfirmDeleteRoundTripComponent } from './confirm-delete-round-trip/confirm-delete-round-trip.component';
import { NbDialogService } from '@nebular/theme';
import { TrainCompanyComponent } from '../../railway/train-company/train-company.component';

@Component({
  selector: 'ngx-round-trip',
  templateUrl: './round-trip.component.html',
  styleUrls: ['./round-trip.component.scss']
})
export class RoundTripComponent implements OnInit {
  roundTrips: RoundTrip [] = [];
  pagination : Pagination;

  currentSearchTerm = '';
  currentSort = '';

  sortStates: {[key: string] : boolean} =
  {
    name : false,
    trainCompanyName: false,
    createdAt : false,
  }

  queryParams: QueryParams =
  {
    pageNumber: 1,
    pageSize: 5,
    searchTerm: '',
    sort: '',
  }


  constructor(private roundTripService : RoundTripService,
              private dialogService : NbDialogService){}

  ngOnInit(): void
  {
    this.getAllRoundTrip();
  }

  getAllRoundTrip(){
    this.roundTripService.getAllRoundTrip(this.queryParams).subscribe({
      next: (res : PaginatedResult<RoundTrip[]>) => {
        this.roundTrips = res.result;
        this.pagination = res.pagination;
      },
    });
  }

  onSearch()
  {
    this.queryParams.searchTerm = this.currentSearchTerm;
    this.queryParams.pageNumber = 1;
    this.getAllRoundTrip();
  }

  onResetSearch()
  {
    this.currentSearchTerm = '';
    this.queryParams.searchTerm = '';
    this.getAllRoundTrip();
  }

  onSort(sort: string)
  {
    const sortType = sort.split('Asc')[0];

    if(this.queryParams.sort === sort)
    {
      this.queryParams.sort = sort.endsWith('Asc') ? sort.replace('Asc', 'Desc') : sort.replace('Desc', 'Asc');
      this.sortStates[sortType] = !this.sortStates[sortType];
    } else
    {
      this.queryParams.sort = sort;
      for (const key in this.sortStates)
      {
        if (this.sortStates.hasOwnProperty(key))
        {
          this.sortStates[key] = false;
        }
      }

      this.sortStates[sortType] = sort.endsWith('Asc');
    }

    this.currentSort = this.queryParams.sort;

    this.queryParams.pageNumber = 1;
    this.getAllRoundTrip();
  }

  openShowDialog(id: number) {
    this.roundTripService.getRoundTripById(id).subscribe({
      next: (res: RoundTrip) => {
        const dialogRef = this.dialogService.open(ShowRoundTripComponent, {
          context: {
            id: res.id,
            trainCompanyId: res.trainCompanyId,
            trainCompanyName: res.trainCompanyName,
            discount: res.discount,
            createdAt: res.createdAt,
          },
        });

        dialogRef.componentRef.instance.onShowDelete.subscribe(obj => {
          this.openConfirmDialog(obj.id, obj.name);
        });
      },
    });
  }

  openConfirmDialog(id: number, name: string) {
    const dialogRef = this.dialogService.open(ConfirmDeleteRoundTripComponent, {
      context: {
        id,
        name,
      },
    });

    dialogRef.componentRef.instance.onConfirmDelete.subscribe((_: any) => {
      this.getAllRoundTrip();
    });
  }

  onPageChanged(newPage: number)
  {
    this.queryParams.pageNumber = newPage;
    this.getAllRoundTrip();
  }

  onPageSizeChanged(newSize : number)
  {
    this.queryParams.pageSize = newSize;
    this.getAllRoundTrip();
  }
}
