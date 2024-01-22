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

    return this.paginationService.getPaginatedResult<TrainCompany[]>(this.baseUrl + '/trainCompanies', params);
  }

  getAllTrainCompanyNoPaging() {
    return this.http.get<TrainCompany[]>(this.baseUrl + '/trainCompanies/all');
  }

  addTrainCompany(trainCompany: TrainCompany) {
    return this.http.post<TrainCompany>(this.baseUrl + '/trainCompanies', trainCompany);
  }

  addTrainCompanyWithLogo(trainCompany: TrainCompany, logo: File) {
    const formData = new FormData();
    formData.append('name', trainCompany.name);
    formData.append('logo', logo);
    formData.append('status', trainCompany.status);
    return this.http.post<TrainCompany>(this.baseUrl + '/trainCompanies/create', formData);
  }

  getTrainCompanyById(id: number) {
    return this.http.get<TrainCompany>(this.baseUrl + '/trainCompanies/' + id);
  }

  updateTrainCompany(trainCompany: TrainCompany) {
    return this.http.put<TrainCompany>(this.baseUrl + '/trainCompanies/' + trainCompany.id, trainCompany);
  }

  updateTrainCompanyWithLogo(trainCompany: TrainCompany, logo: File) {
    const formData = new FormData();
    formData.append('id', trainCompany.id.toString());
    formData.append('name', trainCompany.name);
    formData.append('logo', logo);
    formData.append('status', trainCompany.status);
    return this.http.put<TrainCompany>(this.baseUrl + '/trainCompanies/update/' + trainCompany.id, formData);
  }

  deleteTrainCompany(id: number) {
    return this.http.patch(this.baseUrl + '/trainCompanies/' + id, {});
  }
}
