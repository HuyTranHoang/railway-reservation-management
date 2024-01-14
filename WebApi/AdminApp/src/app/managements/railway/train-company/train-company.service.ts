import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {environment} from '../../../../environments/environment';
import {TrainCompany} from '../../../@models/trainCompany';
import {QueryParams} from '../../../@models/params/queryParams';
import {PaginationService} from '../../shared/pagination.service';

@Injectable({
  providedIn: 'root',
})
export class TrainCompanyService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient, private paginationService: PaginationService) {
  }

  getAllTrainCompany(queryParams: QueryParams) {
    let params = this.paginationService.getPaginationHeaders(queryParams.pageNumber, queryParams.pageSize);

    if (queryParams.searchTerm) {
      params = params.append('searchTerm', queryParams.searchTerm);
    }

    if (queryParams.sort) {
      params = params.append('sort', queryParams.sort);
    }

    return this.paginationService.getPaginatedResult<TrainCompany[]>(this.baseUrl + '/traincompanies', params);
  }

  getAllTrainCompanyNoPaging() {
    return this.http.get<TrainCompany[]>(this.baseUrl + '/traincompanies/all');
  }

  addTrainCompany(trainCompany: TrainCompany) {
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
