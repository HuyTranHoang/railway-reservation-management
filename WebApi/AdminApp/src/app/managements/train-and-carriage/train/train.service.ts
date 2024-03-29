import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {environment} from '../../../../environments/environment';
import {Train} from '../../../@models/train';
import {PaginationService} from '../../shared/pagination.service';
import {TrainQueryParams} from '../../../@models/params/trainQueryParams';

@Injectable({
  providedIn: 'root',
})
export class TrainService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient, private paginationService: PaginationService) {
  }

  getAllTrain(queryParams: TrainQueryParams) {
    let params = this.paginationService
      .getPaginationHeaders(queryParams.pageNumber, queryParams.pageSize);

    if (queryParams.trainCompanyId) {
      params = params.append('TrainCompanyId', queryParams.trainCompanyId);
    }

    if (queryParams.searchTerm) {
      params = params.append('searchTerm', queryParams.searchTerm);
    }

    params = params.append('sort', queryParams.sort);

    return this.paginationService
      .getPaginatedResult<Train[]>(this.baseUrl + '/trains', params);
  }

  getAllTrainNoPaging() {
    return this.http.get<Train[]>(this.baseUrl + '/trains/all');
  }

  getTrainById(id: number) {
    return this.http.get<Train>(this.baseUrl + '/trains/' + id);
  }

  addTrain(train: Train) {
    return this.http.post<Train>(this.baseUrl + '/trains', train);
  }

  updateTrain(train: Train) {
    return this.http.put<Train>(this.baseUrl + '/trains/' + train.id, train);
  }

  deleteTrain(id: number) {
    return this.http.patch(this.baseUrl + '/trains/' + id, {});
  }

}
