import {Injectable} from '@angular/core';
import {environment} from '../../../../environments/environment';
import {HttpClient, HttpParams} from '@angular/common/http';
import {QueryParams} from '../../../@models/params/queryParams';
import {TrainStation} from '../../../@models/trainStation';
import {map} from 'rxjs/operators';
import {PaginatedResult} from '../../../@models/paginatedResult';

@Injectable({
  providedIn: 'root',
})
export class TrainStationService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {
  }

  getAllTrainStation(queryParams: QueryParams) {
    let params = this.getPaginationHeaders(queryParams.pageNumber, queryParams.pageSize);

    if (queryParams.searchTerm) {
      params = params.append('searchTerm', queryParams.searchTerm);
    }

    if (queryParams.sort) {
      params = params.append('sort', queryParams.sort);
    }

    return this.getPaginatedResult<TrainStation[]>(this.baseUrl + '/trainstation/', params);
  }

  getAllTrainStationNoPaging() {
    return this.http.get<TrainStation[]>(this.baseUrl + '/trainstation/all');
  }

  getTrainStationById(id: number) {
    return this.http.get<TrainStation>(this.baseUrl + '/trainstation/' + id);
  }

  addTrainStation(trainStation: TrainStation) {
    return this.http.post<TrainStation>(this.baseUrl + '/trainstation', trainStation);
  }

  updateTrainStation(trainStation: TrainStation) {
    return this.http.put<TrainStation>(this.baseUrl + '/trainstation/' + trainStation.id, trainStation);
  }

  deleteTrainStation(id: number) {
    return this.http.patch(this.baseUrl + '/trainstation/' + id, {});
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
