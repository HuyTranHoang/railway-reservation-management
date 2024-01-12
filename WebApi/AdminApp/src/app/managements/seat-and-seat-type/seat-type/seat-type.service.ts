import {Injectable} from '@angular/core';
import {environment} from '../../../../environments/environment';
import {HttpClient, HttpParams} from '@angular/common/http';
import {QueryParams} from '../../../@models/params/queryParams';
import {SeatType} from '../../../@models/seatType';
import {PaginatedResult} from '../../../@models/paginatedResult';
import {map} from 'rxjs/internal/operators/map';
import {PaginationService} from '../../shared/pagination.service';

@Injectable({
  providedIn: 'root',
})
export class SeatTypeService {

  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient, private paginationService: PaginationService) {
  }

  getAllSeatType(queryParams: QueryParams) {
    let params = this.paginationService.getPaginationHeaders(queryParams.pageNumber, queryParams.pageSize);

    if (queryParams.searchTerm) {
      params = params.append('searchTerm', queryParams.searchTerm);
    }

    if (queryParams.sort) {
      params = params.append('sort', queryParams.sort);
    }

    return this.paginationService.getPaginatedResult<SeatType[]>(this.baseUrl + '/seatTypes', params);
  }

  getSeatTypeById(id: number) {
    return this.http.get<SeatType>(this.baseUrl + '/seatTypes/' + id);
  }

  addSeatType(seatType: SeatType) {
    return this.http.post<SeatType>(this.baseUrl + '/seatTypes', seatType);
  }

  updateSeatType(seatType: SeatType) {
    return this.http.put<SeatType>(this.baseUrl + '/seatTypes/' + seatType.id, seatType);
  }

  deleteSeatType(id: number) {
    return this.http.patch(this.baseUrl + '/seatTypes/' + id, {});
  }

}
