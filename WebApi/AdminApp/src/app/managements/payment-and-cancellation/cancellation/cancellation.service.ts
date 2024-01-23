import {Injectable} from '@angular/core';
import {environment} from '../../../../environments/environment';
import {HttpClient} from '@angular/common/http';
import {PaginationService} from '../../shared/pagination.service';
import {QueryParams} from '../../../@models/params/queryParams';
import {Cancellation} from '../../../@models/cancellation';

@Injectable({
  providedIn: 'root'
})
export class CancellationService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient, private paginationService: PaginationService) {
  }

  getAllCancellation(queryParams: QueryParams) {
    let params = this.paginationService.getPaginationHeaders(queryParams.pageNumber, queryParams.pageSize);

    if (queryParams.searchTerm) {
      params = params.append('searchTerm', queryParams.searchTerm);
    }

    if (queryParams.sort) {
      params = params.append('sort', queryParams.sort);
    }

    return this.paginationService.getPaginatedResult<Cancellation[]>(
      this.baseUrl + '/cancellation', params);
  }

  getAllCancellationNoPaging() {
    return this.http.get<Cancellation[]>(this.baseUrl + '/cancellation/all');
  }

  addCancellation(cancellation: Cancellation) {
    return this.http.post<Cancellation>(this.baseUrl + '/cancellation', cancellation);
  }

  getCancellationById(id: number) {
    return this.http.get<Cancellation>(this.baseUrl + '/cancellation/' + id);
  }

  updateCancellation(cancellation: Cancellation) {
    return this.http.put<Cancellation>(
      this.baseUrl + '/cancellation/' + cancellation.id, cancellation);
  }

  deleteCancellation(id: number) {
    return this.http.patch(this.baseUrl + '/cancellation/' + id, {});
  }
}
