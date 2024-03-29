import {Component, OnInit} from '@angular/core';
import {Pagination} from '../../../@models/pagination';
import {Passenger} from '../../../@models/passenger';
import {QueryParams} from '../../../@models/params/queryParams';
import {PassengerService} from './passenger.service';
import {NbDialogService} from '@nebular/theme';
import {ConfirmDeletePassengerComponent} from './confirm-delete-passenger/confirm-delete-passenger.component';
import {ShowPassengerComponent} from './show-passenger/show-passenger.component';
import {SharedService} from '../../shared/shared.service';

@Component({
  selector: 'ngx-passenger',
  templateUrl: './passenger.component.html',
  styleUrls: ['./passenger.component.scss'],
})
export class PassengerComponent implements OnInit {
  passengers: Passenger[] = [];
  pagination: Pagination;

  currentSearchTerm: string = '';
  currentSort: string = '';

  queryParams: QueryParams = {
    pageNumber: 1,
    pageSize: 10,
    searchTerm: '',
    sort: '',
  };

  sortStates = {
    fullName: false,
    cardId: false,
    age: false,
    email: false,
    createdAt: false,
  };

  constructor(private passengerService: PassengerService,
              private sharedService: SharedService,
              private dialogService: NbDialogService) {
  }

  ngOnInit(): void {
    this.getAllPassengers();
  }

  getAllPassengers() {
    this.passengerService.getAllPassengers(this.queryParams).subscribe({
      next: (res: any) => {
        this.passengers = res.result;
        this.pagination = res.pagination;

        this.checkItemsAndAdjustPage();
      },
    });
  }

  checkItemsAndAdjustPage() {
    if (this.passengers.length === 0 && this.pagination.currentPage > 1) {
      this.queryParams.pageNumber = this.pagination.currentPage - 1;
      this.getAllPassengers();
    }
  }

  onSearch() {
    this.queryParams.searchTerm = this.currentSearchTerm;
    this.queryParams.pageNumber = 1;
    this.getAllPassengers();
  }

  onResetSearch() {
    this.currentSearchTerm = '';
    this.queryParams.searchTerm = '';
    this.getAllPassengers();
  }

  onSort(sort: string) {
    const result = this.sharedService.sortItems(this.queryParams, sort, this.sortStates);
    this.queryParams = result.queryParams;
    this.sortStates = result.sortStates;
    this.currentSort = this.queryParams.sort;
    this.getAllPassengers();
  }

  openShowDialog(id: number) {
    const passenger = this.passengers.find(p => p.id === id);

    const dialogRef = this.dialogService.open(ShowPassengerComponent, {
      context: {...passenger},
    });

    dialogRef.componentRef.instance.onShowDelete.subscribe(obj => {
      this.openConfirmDialog(obj.id, obj.name);
    });
  }

  openConfirmDialog(id: number, name: string) {
    const dialogRef = this.dialogService.open(ConfirmDeletePassengerComponent, {
      context: {id, name},
    });

    dialogRef.componentRef.instance.onConfirmDelete.subscribe(_ => {
      this.getAllPassengers();
    });
  }

  onPageChanged(newPage: number) {
    this.queryParams.pageNumber = newPage;
    this.getAllPassengers();
  }

  onPageSizeChanged(newSize: number) {
    this.queryParams.pageSize = newSize;
    this.getAllPassengers();
  }

}
