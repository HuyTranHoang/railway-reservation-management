import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {environment} from '../../../environments/environment';
import {DailyCashTransaction} from '../../@models/dailyCashTransaction';
import {QueryParams} from '../../@models/params/queryParams';
import {PaginationService} from '../shared/pagination.service';

@Injectable({
  providedIn: 'root',
})


export class DailyTransactionService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient, private paginationService: PaginationService) {
  }

  getAllDailyCashTransaction(queryParams: QueryParams) {
    let params = this.paginationService
      .getPaginationHeaders(queryParams.pageNumber, queryParams.pageSize);

    if (queryParams.searchTerm) {
      params = params.append('searchTerm', queryParams.searchTerm);
    }

    if (queryParams.sort) {
      params = params.append('sort', queryParams.sort);
    }

    return this.paginationService
      .getPaginatedResult<DailyCashTransaction[]>(this.baseUrl + '/dailycashtransactions', params);
  }

  getAllDailyCashTransactionNoPaging() {
    return this.http.get<DailyCashTransaction[]>(this.baseUrl + '/dailycashtransactions/all');
  }

  getDailyCashTransactionById(id: number) {
    return this.http.get<DailyCashTransaction>(this.baseUrl + '/dailycashtransactions/' + id);
  }
}
