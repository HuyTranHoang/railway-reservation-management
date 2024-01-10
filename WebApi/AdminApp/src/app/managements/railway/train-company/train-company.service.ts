import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { PaginatedResult } from '../../../@models/paginatedResult';
import { TrainCompany } from '../../../@models/trainCompany';
import { QueryParams } from '../../../@models/params/queryParams';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class TrainCompanyService {
  baseUrl = environment.apiUrl;

  constructor(private http : HttpClient) { }

  getAllTrainCompany(queryParams : QueryParams){
    let params = this.getPaginationHeaders(queryParams.pageNumber,queryParams.pageSize);

    if (queryParams.searchTerm) {
      params = params.append('searchTerm', queryParams.searchTerm);
    }

    if (queryParams.sort) {
      params = params.append('sort', queryParams.sort);
    }

    return this.getPaginatedResult<TrainCompany[]>(this.baseUrl + '/traincompanies', params);
  }

  private getPaginatedResult<T>(url: string, params: HttpParams){
    const paginatedResult: PaginatedResult<T> = new PaginatedResult<T>();

    return this.http.get<T>(url, {observe: 'response', params}).pipe(
      map(response => {
        if(response.body){
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

  private getPaginationHeaders(pageNumber: number, pageSize: number){
    let params = new HttpParams();

    params = params.append('pageNumber', pageNumber);
    params = params.append('pageSize', pageSize);

    return params;
  }

  addTrainCompany(trainCompany : TrainCompany){
    return this.http.post<TrainCompany>(this.baseUrl + '/traincompanies', trainCompany);
  }

  getTrainCompanyById(id: number) {
    return this.http.get<TrainCompany>(this.baseUrl + '/traincompanies/' + id);
  }

  updateTrainCompany(trainCompany: TrainCompany) {
    return this.http.put<TrainCompany>(this.baseUrl + '/traincompanies/' + trainCompany.id, trainCompany);
  }

  deleteTrainCompany(id: number) {
    return this.http.patch(this.baseUrl + '/traincompanies/' + id, {});
  }
}
