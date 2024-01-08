import {Component, OnInit} from '@angular/core';
import {CarriageTypeService} from './carriage-type.service';
import {CarriageType} from '../../../@models/carriageType';
import {Pagination} from '../../../@models/pagination';
import {PaginatedResult} from '../../../@models/paginatedResult';
import {QueryParams} from '../../../@models/params/queryParams';

@Component({
  selector: 'ngx-carriage-type',
  templateUrl: './carriage-type.component.html',
  styleUrls: ['./carriage-type.component.scss'],
})
export class CarriageTypeComponent implements OnInit {

  carriageTypes: CarriageType[] = [];
  pagination: Pagination;
  queryParams: QueryParams = {
    pageNumber: 1,
    pageSize: 10,
    searchTerm: '',
    sort: '',
  };

  constructor(private carriageTypeService: CarriageTypeService) {
  }

  ngOnInit(): void {
    this.getAllCarriageType();
  }

  getAllCarriageType() {
    this.carriageTypeService.getAllCarriageType(this.queryParams).subscribe({
      next: (res: PaginatedResult<CarriageType[]>) => {
        this.carriageTypes = res.result;
        this.pagination = res.pagination;
      },
      error: (err) => {
      },
    });
  }
}
