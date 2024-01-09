import { Injectable } from '@angular/core';
import {HttpClient, HttpParams} from '@angular/common/http';
import {PaginatedResult} from '../../@models/paginatedResult';
import {map} from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class PaginationService {

  constructor(private http: HttpClient) { }

  public getPaginatedResult<T>(url: string, params: HttpParams) {
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

  public getPaginationHeaders(pageNumber: number, pageSize: number) {
    let params = new HttpParams();

    params = params.append('pageNumber', pageNumber);
    params = params.append('pageSize', pageSize);

    return params;
  }
}
