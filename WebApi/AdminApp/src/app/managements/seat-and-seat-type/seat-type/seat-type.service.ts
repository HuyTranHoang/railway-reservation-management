import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { QueryParams } from '../../../@models/params/queryParams';
import { SeatType } from '../../../@models/seatType';
import { PaginatedResult } from '../../../@models/paginatedResult';
import { map } from 'rxjs/internal/operators/map';

@Injectable({
  providedIn: 'root'
})
export class SeatTypeService {

  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {
  }

  getAllSeatType(queryParams: QueryParams) {
    let params = this.getPaginationHeaders(queryParams.pageNumber, queryParams.pageSize);

    if (queryParams.searchTerm) {
      params = params.append('searchTerm', queryParams.searchTerm);
    }

    if (queryParams.sort) {
      params = params.append('sort', queryParams.sort);
    }

    return this.getPaginatedResult<SeatType[]>(this.baseUrl + '/seatTypes', params);
  }

  getSeatTypeById(id: number) {
    return this.http.get<SeatType>(this.baseUrl + '/seatTypes/' + id);
  }

  addSeatType(seatType: SeatType) {
    return this.http.post<SeatType>(this.baseUrl + '/seatTypes', seatType);
  }
  updateSeatType( seatType: SeatType) {
    return this.http.put<SeatType>(this.baseUrl + '/seatTypes/' + seatType.id , seatType);
  }
  deleteSeatType(id: number) {
    return this.http.patch(this.baseUrl + '/seatTypes/' + id, {});
  }

  private getPaginatedResult<T>(url: string, params: HttpParams) {
    const paginatedResult: PaginatedResult<T> = new PaginatedResult<T>();

    return this.http.get<T>(url, {observe: 'response', params}).pipe(
      map(response => {
        if (response.body) {
          paginatedResult.result = response.body;
        }

        const pagination = response.headers.get('Pagination');
        if (pagination) {
          paginatedResult.pagination = JSON.parse(pagination);
        }
        return paginatedResult;
      }),
    );
  }

  private getPaginationHeaders(pageNumber: number, pageSize: number) {
    let params = new HttpParams();

    params = params.append('pageNumber', pageNumber);
    params = params.append('pageSize', pageSize);

    return params;
  }
}
