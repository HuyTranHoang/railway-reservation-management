import {Component, OnInit} from '@angular/core';
import {Pagination} from '../../../@models/pagination';
import {NbDialogService} from '@nebular/theme';
import {PaginatedResult} from '../../../@models/paginatedResult';
import {Payment} from '../../../@models/payment';
import {PaymentService} from './payment.service';
import {PaymentQueryParams} from '../../../@models/params/paymentQueryParams';
import {ShowPaymentComponent} from './show-payment/show-payment.component';
import {ConfirmDeletePaymentComponent} from './confirm-delete-payment/confirm-delete-payment.component';
import {SharedService} from '../../shared/shared.service';

@Component({
  selector: 'ngx-payment',
  templateUrl: './payment.component.html',
  styleUrls: ['./payment.component.scss'],
})
export class PaymentComponent implements OnInit {

  payments: Payment[] = [];
  pagination: Pagination;

  currentSearchTerm: string = '';
  currentSort: string = '';

  sortStates = {
    email: false,
    createdAt: false,
  };

  queryParams: PaymentQueryParams = {
    pageNumber: 1,
    pageSize: 10,
    searchTerm: '',
    sort: '',
    createdAt: '',
  };

  constructor(private paymentService: PaymentService,
              private sharedService: SharedService,
              private dialogService: NbDialogService) {
  }

  ngOnInit(): void {
    this.getAllPayments();
  }

  getAllPayments() {
    this.paymentService.getAllPayment(this.queryParams).subscribe({
      next: (res: PaginatedResult<Payment[]>) => {
        this.payments = res.result;
        this.pagination = res.pagination;

        this.checkItemsAndAdjustPage();
      },
    });
  }

  checkItemsAndAdjustPage() {
    if (this.payments.length === 0 && this.pagination.currentPage > 1) {
      this.queryParams.pageNumber = this.pagination.currentPage - 1;
      this.getAllPayments();
    }
  }


  onSearch() {
    this.queryParams.searchTerm = this.currentSearchTerm;
    this.queryParams.pageNumber = 1;
    this.getAllPayments();
  }

  onResetSearch() {
    this.currentSearchTerm = '';
    this.queryParams.searchTerm = '';
    this.getAllPayments();
  }

  onSort(sort: string) {
    const result = this.sharedService.sortItems(this.queryParams, sort, this.sortStates);
    this.queryParams = result.queryParams;
    this.sortStates = result.sortStates;
    this.currentSort = this.queryParams.sort;
    this.getAllPayments();
  }


  openShowDialog(id: number) {
    const payment = this.payments.find(x => x.id === id);

    const dialogRef = this.dialogService.open(ShowPaymentComponent, {
      context: {...payment},
    });

    dialogRef.componentRef.instance.onShowDelete.subscribe(obj => {
      this.openConfirmDialog(obj.id, obj.name, obj.transactionId);
    });
  }

  openConfirmDialog(id: number, name: string, transactionId: string) {
    const dialogRef = this.dialogService.open(ConfirmDeletePaymentComponent, {
      context: {id, name, transactionId},
    });

    dialogRef.componentRef.instance.onConfirmDelete.subscribe(_ => {
      this.getAllPayments();
    });
  }

  onPageChanged(newPage: number) {
    this.queryParams.pageNumber = newPage;
    this.getAllPayments();
  }

  onPageSizeChanged(newSize: number) {
    this.queryParams.pageSize = newSize;
    this.getAllPayments();
  }
}
