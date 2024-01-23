import {Component, OnInit} from '@angular/core';
import {Pagination} from '../../../../@models/pagination';
import {SharedService} from '../../../shared/shared.service';
import {NbDialogService} from '@nebular/theme';
import {PaginatedResult} from '../../../../@models/paginatedResult';
import {TicketService} from '../ticket.service';
import {Ticket} from '../../../../@models/ticket';
import {TicketQueryParams} from '../../../../@models/params/ticketQueryParams';

@Component({
  selector: 'ngx-list-ticket',
  templateUrl: './list-ticket.component.html',
  styleUrls: ['./list-ticket.component.scss'],
})
export class ListTicketComponent implements OnInit {
  tickets: Ticket[] = [];
  pagination: Pagination;

  currentSearchTerm: string = '';
  currentSort: string = '';

  sortStates = {
    name: false,
    numberOfCompartment: false,
    serviceCharge: false,
    createdAt: false,
  };

  queryParams: TicketQueryParams = {
    pageNumber: 1,
    pageSize: 10,
    searchTerm: '',
    sort: '',
    createdAt: '',
    passengerId: 0,
    trainId: 0,
    distanceFareId: 0,
    carriageId: 0,
    seatId: 0,
    scheduleId: 0,
    paymentId: 0,
  };

  constructor(private ticketService: TicketService,
              private sharedService: SharedService,
              private dialogService: NbDialogService) {
  }

  ngOnInit(): void {
    this.loadAllTickets();
  }

  loadAllTickets() {
    this.ticketService.getAllTicket(this.queryParams).subscribe({
      next: (res: PaginatedResult<Ticket[]>) => {
        this.tickets = res.result;
        this.pagination = res.pagination;

        this.checkItemsAndAdjustPage();
      },
    });
  }

  checkItemsAndAdjustPage() {
    if (this.tickets.length === 0 && this.pagination.currentPage > 1) {
      this.queryParams.pageNumber = this.pagination.currentPage - 1;
      this.loadAllTickets();
    }
  }

  onSearch() {
    this.queryParams.searchTerm = this.currentSearchTerm;
    this.queryParams.pageNumber = 1;
    this.loadAllTickets();
  }

  onResetSearch() {
    this.currentSearchTerm = '';
    this.queryParams.searchTerm = '';
    this.loadAllTickets();
  }

  onSort(sort: string) {
    const result = this.sharedService.sortItems(this.queryParams, sort, this.sortStates);
    this.queryParams = result.queryParams;
    this.sortStates = result.sortStates;
    this.currentSort = this.queryParams.sort;
    this.loadAllTickets();
  }

  openShowDialog(id: number) {
    // const carriageType = this.carriageTypes.find(x => x.id === id);
    //
    // const dialogRef = this.dialogService.open(ShowCarriageTypeComponent, {
    //   context: {...carriageType},
    // });
    //
    // dialogRef.componentRef.instance.onShowDelete.subscribe(obj => {
    //   this.openConfirmDialog(obj.id, obj.name);
    // });

  }

  openConfirmDialog(id: number, name: string) {
    // const dialogRef = this.dialogService.open(ConfirmDeleteCarriageTypeComponent, {
    //   context: { id, name },
    // });
    //
    // dialogRef.componentRef.instance.onConfirmDelete.subscribe(_ => {
    //   this.getAllCarriageType();
    // });
  }

  onPageChanged(newPage: number) {
    this.queryParams.pageNumber = newPage;
    this.loadAllTickets();
  }

  onPageSizeChanged(newSize: number) {
    this.queryParams.pageSize = newSize;
    this.loadAllTickets();
  }
}
