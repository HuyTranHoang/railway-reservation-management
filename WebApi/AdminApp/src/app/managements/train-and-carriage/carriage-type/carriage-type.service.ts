import {Injectable} from '@angular/core';
import {HttpClient, HttpParams} from '@angular/common/http';
import {environment} from '../../../../environments/environment';
import {CarriageType} from '../../../@models/carriageType';
import {PaginatedResult} from '../../../@models/paginatedResult';
import {map} from 'rxjs/operators';
import {QueryParams} from '../../../@models/params/queryParams';
import {PaginationService} from '../../shared/pagination.service';

@Injectable({
  providedIn: 'root',
})


export class CarriageTypeService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient, private paginationService: PaginationService) {
  }

  getAllCarriageType(queryParams: QueryParams) {
    let params = this.paginationService
      .getPaginationHeaders(queryParams.pageNumber, queryParams.pageSize);

    if (queryParams.searchTerm) {
      params = params.append('searchTerm', queryParams.searchTerm);
    }

    if (queryParams.sort) {
      params = params.append('sort', queryParams.sort);
    }

    return this.paginationService
      .getPaginatedResult<CarriageType[]>(this.baseUrl + '/carriageTypes', params);
  }

  getCarriageTypeById(id: number) {
    return this.http.get<CarriageType>(this.baseUrl + '/carriageTypes/' + id);
  }

  addCarriageType(carriageType: CarriageType) {
    return this.http.post<CarriageType>(this.baseUrl + '/carriageTypes', carriageType);
  }

  updateCarriageType(carriageType: CarriageType) {
    return this.http.put<CarriageType>(this.baseUrl + '/carriageTypes/' + carriageType.id, carriageType);
  }

  deleteCarriageType(id: number) {
    return this.http.patch(this.baseUrl + '/carriageTypes/' + id, {});
  }



}
