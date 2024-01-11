import {Component, OnInit} from '@angular/core';
import {Pagination} from '../../../@models/pagination';
import {NbDialogService} from '@nebular/theme';
import {PaginatedResult} from '../../../@models/paginatedResult';
import {Payment} from '../../../@models/payment';
import {PaymentService} from './payment.service';
import {PaymentQueryParams} from '../../../@models/params/paymentQueryParams';
import {ShowPaymentComponent} from './show-payment/show-payment.component';
import {ConfirmDeletePaymentComponent} from './confirm-delete-payment/confirm-delete-payment.component';

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

  // Trong component của bạn
  sortStates = {
    email: false,
    createdAt: false,
  };

  queryParams: PaymentQueryParams = {
    pageNumber: 1,
    pageSize: 5,
    searchTerm: '',
    sort: '',
    createdAt: '',
  };

  constructor(private paymentService: PaymentService, private dialogService: NbDialogService) {
  }

  ngOnInit(): void {
    this.getAllPayments();
  }

  getAllPayments() {
    this.paymentService.getAllPayment(this.queryParams).subscribe({
      next: (res: PaginatedResult<Payment[]>) => {
        this.payments = res.result;
        this.pagination = res.pagination;
      },
    });
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
    this.getAllPayments();
  }


  openShowDialog(id: number) {
    this.paymentService.getPaymentById(id).subscribe({
      next: (res: Payment) => {
        const dialogRef = this.dialogService.open(ShowPaymentComponent, {
          context: {
            id: res.id,
            fullName: res.aspNetUserFullName,
            email: res.aspNetUserEmail,
            status: res.status,
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
    const dialogRef = this.dialogService.open(ConfirmDeletePaymentComponent, {
      context: {id, name},
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
