import {Injectable} from '@angular/core';
import {HttpClient, HttpParams} from '@angular/common/http';
import {environment} from '../../../../environments/environment';
import {CarriageType} from '../../../@models/carriageType';
import {PaginatedResult} from '../../../@models/paginatedResult';
import {map} from 'rxjs/operators';
import {QueryParams} from '../../../@models/params/queryParams';

@Injectable({
  providedIn: 'root',
})


export class CarriageTypeService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {
  }

  getAllCarriageType(queryParams: QueryParams) {
    let params = this.getPaginationHeaders(queryParams.pageNumber, queryParams.pageSize);

    if (queryParams.searchTerm) {
      params = params.append('searchTerm', queryParams.searchTerm);
    }

    if (queryParams.sort) {
      params = params.append('sort', queryParams.sort);
    }

    return this.getPaginatedResult<CarriageType[]>(this.baseUrl + '/carriageTypes', params);
  }

  addCarriageType(carriageType: CarriageType) {
    return this.http.post<CarriageType>(this.baseUrl + '/carriageTypes', carriageType);
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
