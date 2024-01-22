import {Component, OnInit} from '@angular/core';
import {Pagination} from '../../../@models/pagination';
import {QueryParams} from '../../../@models/params/queryParams';
import {SharedService} from '../../shared/shared.service';
import {NbDialogService} from '@nebular/theme';
import {PaginatedResult} from '../../../@models/paginatedResult';
import {UserService} from '../user.service';
import {User} from '../../../@models/auth/applicationUser';
import {ConfirmLockUserComponent} from '../confirm-lock-user/confirm-lock-user.component';

@Component({
  selector: 'ngx-list-user',
  templateUrl: './list-user.component.html',
  styleUrls: ['./list-user.component.scss'],
})
export class ListUserComponent implements OnInit {
  users: User[] = [];
  pagination: Pagination;

  currentSearchTerm: string = '';
  currentSort: string = '';

  sortStates = {
    firstName: false,
    lastName: false,
    email: false,
    phoneNumber: false,
    isLocked: false,
    createdAt: false,
  };

  queryParams: QueryParams = {
    pageNumber: 1,
    pageSize: 10,
    searchTerm: '',
    sort: '',
  };

  constructor(private userService: UserService,
              private sharedService: SharedService,
              private dialogService: NbDialogService) {
  }

  ngOnInit(): void {
    this.loadAllUsers();
  }

  loadAllUsers() {
    this.userService.getAllUser(this.queryParams).subscribe({
      next: (res: PaginatedResult<User[]>) => {
        this.users = res.result;
        this.pagination = res.pagination;

        this.checkItemsAndAdjustPage();
      },
    });
  }

  checkItemsAndAdjustPage() {
    if (this.users.length === 0 && this.pagination.currentPage > 1) {
      this.queryParams.pageNumber = this.pagination.currentPage - 1;
      this.loadAllUsers();
    }
  }

  onSearch() {
    this.queryParams.searchTerm = this.currentSearchTerm;
    this.queryParams.pageNumber = 1;
    this.loadAllUsers();
  }

  onResetSearch() {
    this.currentSearchTerm = '';
    this.queryParams.searchTerm = '';
    this.loadAllUsers();
  }

  onSort(sort: string) {
    const result = this.sharedService.sortItems(this.queryParams, sort, this.sortStates);
    this.queryParams = result.queryParams;
    this.sortStates = result.sortStates;
    this.currentSort = this.queryParams.sort;
    this.loadAllUsers();
  }

  // openShowDialog(email: string) {
  //   const user = this.users.find(x => x.email === email);
  //
  //   const dialogRef = this.dialogService.open(ShowCarriageTypeComponent, {
  //     context: {...carriageType},
  //   });
  //
  //   dialogRef.componentRef.instance.onShowDelete.subscribe(obj => {
  //     this.openConfirmDialog(obj.id, obj.name);
  //   });
  // }

  openConfirmDialog(id: string, fullName: string, isLocked: boolean) {
    const dialogRef = this.dialogService.open(ConfirmLockUserComponent, {
      context: { id, fullName, isLocked },
    });

    dialogRef.componentRef.instance.onConfirmLock.subscribe(_ => {
      this.loadAllUsers();
    });
  }

  onPageChanged(newPage: number) {
    this.queryParams.pageNumber = newPage;
    this.loadAllUsers();
  }

  onPageSizeChanged(newSize: number) {
    this.queryParams.pageSize = newSize;
    this.loadAllUsers();
  }

}
