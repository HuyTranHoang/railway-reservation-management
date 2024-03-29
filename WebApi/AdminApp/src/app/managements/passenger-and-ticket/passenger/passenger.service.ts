import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {environment} from '../../../../environments/environment';
import {QueryParams} from '../../../@models/params/queryParams';
import {CarriageType} from '../../../@models/carriageType';
import {PaginationService} from '../../shared/pagination.service';
import {Passenger} from '../../../@models/passenger';

@Injectable({
  providedIn: 'root',
})
export class PassengerService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient, private paginationService: PaginationService) {
  }

  getAllPassengers(queryParams: QueryParams) {
    let params = this.paginationService
      .getPaginationHeaders(queryParams.pageNumber, queryParams.pageSize);

    if (queryParams.searchTerm) {
      params = params.append('searchTerm', queryParams.searchTerm);
    }

    if (queryParams.sort) {
      params = params.append('sort', queryParams.sort);
    }

    return this.paginationService
      .getPaginatedResult<CarriageType[]>(this.baseUrl + '/passengers', params);
  }

  getPassengerById(id: number) {
    return this.http.get<Passenger>(this.baseUrl + '/passengers/' + id);
  }

  addPassenger(model: Passenger) {
    return this.http.post(this.baseUrl + '/passengers', model);
  }

  updatePassenger(model: Passenger) {
    return this.http.put(this.baseUrl + '/passengers/' + model.id, model);
  }

  deletePassenger(id: number) {
    return this.http.patch(this.baseUrl + '/passengers/' + id, {});
  }


}
