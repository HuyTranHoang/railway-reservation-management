import {Injectable} from '@angular/core';
import {environment} from '../../../../environments/environment';
import {HttpClient} from '@angular/common/http';
import {Carriage} from '../../../@models/carriage';
import {CarriageQueryParams} from '../../../@models/params/carriageQueryParams';
import {PaginationService} from '../../shared/pagination.service';

@Injectable({
  providedIn: 'root',
})
export class CarriageService {

  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient, private paginationService: PaginationService) {
  }

  getAllCarriages(queryParams: CarriageQueryParams) {

    let params = this.paginationService
      .getPaginationHeaders(queryParams.pageNumber, queryParams.pageSize);

    if (queryParams.searchTerm) {
      params = params.append('searchTerm', queryParams.searchTerm);
    }

    if (queryParams.sort) {
      params = params.append('sort', queryParams.sort);
    }

    if (queryParams.trainId) {
      params = params.append('trainId', queryParams.trainId);
    }

    if (queryParams.carriageTypeId) {
      params = params.append('carriageTypeId', queryParams.carriageTypeId);
    }

    return this.paginationService
      .getPaginatedResult<Carriage[]>(this.baseUrl + '/carriages', params);

  }

  getCarriageById(id: number) {
    return this.http.get<Carriage>(this.baseUrl + '/carriages/' + id);
  }

  addCarriage(carriage: Carriage) {
    return this.http.post<Carriage>(this.baseUrl + '/carriages', carriage);
  }

  updateCarriage(carriage: Carriage) {
    return this.http.put<Carriage>(this.baseUrl + '/carriages/' + carriage.id, carriage);
  }

  deleteCarriage(id: number) {
    return this.http.patch(this.baseUrl + '/carriages/' + id, {});
  }
}
