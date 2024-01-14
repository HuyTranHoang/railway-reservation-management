import {Component, OnInit} from '@angular/core';
import {Pagination} from '../../../../@models/pagination';
import {NbDialogService} from '@nebular/theme';
import {PaginatedResult} from '../../../../@models/paginatedResult';
import { ConfirmDeleteCompartmentComponent } from '../confirm-delete-compartment/confirm-delete-compartment.component';
import { Compartment } from '../../../../@models/compartment';
import { CompartmentService } from '../compartment.service';
import { CompartmentQueryParams } from '../../../../@models/params/compartmentQueryParams';
import { ShowCompartmentComponent } from '../show-compartment/show-compartment.component';

@Component({
  selector: 'ngx-list-compartment',
  templateUrl: './list-compartment.component.html',
  styleUrls: ['./list-compartment.component.scss']
})
export class ListCompartmentComponent implements OnInit {
  compartments: Compartment[] = [];
  pagination: Pagination;

  currentSearchTerm: string = '';
  currentSort: string = '';

  sortStates = {
    name: false,
    carriageName: false,
    numberOfSeats: false,
    createdAt: false,
  };

  queryParams: CompartmentQueryParams = {
    pageNumber: 1,
    pageSize: 10,
    searchTerm: '',
    sort: '',
    carriageId: 0,
  };

  constructor(private compartmentService: CompartmentService, private dialogService: NbDialogService) {
  }

  ngOnInit(): void {
    this.getAllCompartments();
  }

  getAllCompartments() {
    this.compartmentService.getAllCompartments(this.queryParams).subscribe({
      next: (res: PaginatedResult<Compartment[]>) => {
        this.compartments = res.result;
        this.pagination = res.pagination;

        this.checkItemsAndAdjustPage();
      },
    });
  }

  checkItemsAndAdjustPage() {
    if (this.compartments.length === 0 && this.pagination.currentPage > 1) {
      this.queryParams.pageNumber = this.pagination.currentPage - 1;
      this.getAllCompartments();
    }
  }

  onSearch() {
    this.queryParams.searchTerm = this.currentSearchTerm;
    this.queryParams.pageNumber = 1;
    this.getAllCompartments();
  }

  onResetSearch() {
    this.currentSearchTerm = '';
    this.queryParams.searchTerm = '';
    this.getAllCompartments();
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
    this.getAllCompartments();
  }


  openShowDialog(id: number) {
    const compartment = this.compartments.find(x => x.id === id);

    const dialogRef = this.dialogService.open(ShowCompartmentComponent, {
      context: {...compartment},
    });

    dialogRef.componentRef.instance.onShowDelete.subscribe(obj => {
      this.openConfirmDialog(obj.id, obj.name);
    });

  }

  openConfirmDialog(id: number, name: string) {
    const dialogRef = this.dialogService.open(ConfirmDeleteCompartmentComponent, {
      context: {
        id,
        name,
      },
    });

    dialogRef.componentRef.instance.onConfirmDelete.subscribe(_ => {
      this.getAllCompartments();
    });
  }

  onPageChanged(newPage: number) {
    this.queryParams.pageNumber = newPage;
    this.getAllCompartments();
  }

  onPageSizeChanged(newSize: number) {
    this.queryParams.pageSize = newSize;
    this.getAllCompartments();
  }
}
