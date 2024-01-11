import { Component, OnInit } from '@angular/core';
import { NbDialogService } from '@nebular/theme';
import { PaginatedResult } from '../../../../@models/paginatedResult';
import { Pagination } from '../../../../@models/pagination';
import { QueryParams } from '../../../../@models/params/queryParams';
import { Train } from '../../../../@models/train';
import { TrainService } from '../train.service';
import { ConfirmDeleteTrainComponent } from '../confirm-delete-train/confirm-delete-train.component';

@Component({
  selector: 'ngx-list-train',
  templateUrl: './list-train.component.html',
  styleUrls: ['./list-train.component.scss']
})
export class ListTrainComponent implements OnInit{
  train: Train [] = [];
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
    pageSize: 2,
    searchTerm: '',
    sort: '',
  }


  constructor(private trainService : TrainService,
              private dialogService: NbDialogService){}

  ngOnInit(): void
  {
    this.getAllTrain();
  }

  getAllTrain(){
    this.trainService.getAllTrain(this.queryParams).subscribe({
      next: (res : PaginatedResult<Train[]>) => {
        this.train = res.result;
        this.pagination = res.pagination;
      },
    });
  }

  onSearch()
  {
    this.queryParams.searchTerm = this.currentSearchTerm;
    this.queryParams.pageNumber = 1;
    this.getAllTrain();
  }

  onResetSearch()
  {
    this.currentSearchTerm = '';
    this.queryParams.searchTerm = '';
    this.getAllTrain();
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
    this.getAllTrain();
  }

  onPageChanged(newPage: number)
  {
    this.queryParams.pageNumber = newPage;
    this.getAllTrain();
  }

  onPageSizeChanged(newSize : number)
  {
    this.queryParams.pageSize = newSize;
    this.getAllTrain();
  }

  // openShowDialog(id: number)
  // {
  //   this.trainService.getTrainById(id).subscribe({
  //     next: (res: Train) => {
  //       const dialogRef = this.dialogService.open(ShowTrainCompoment, {
  //         context: {
  //           id: res.id,
  //           name : res.name,
  //           trainCompanyName: res.trainCompanyName,
  //           status: res.status,
  //           createAt : res.CreateAt,
  //         },
  //       });

  //       dialogRef.componentRef.instance.onShowDelete.subscribe(obj => {
  //         this.openConfirmDialog(obj.id, obj.name);
  //       });
  //     }
  //   })
  // }

  openConfirmDialog(id: number, name: string) {
    const dialogRef = this.dialogService.open(ConfirmDeleteTrainComponent, {
      context: {
        id,
        name,
      },
    });

    dialogRef.componentRef.instance.onConfirmDelete.subscribe(_ => {
      this.getAllTrain();
    });
  }

}
