import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {environment} from '../../../../environments/environment';
import {QueryParams} from '../../../@models/params/queryParams';
import {Seat} from '../../../@models/seat';
import {PaginationService} from '../../shared/pagination.service';

@Injectable({
  providedIn: 'root'
})
export class SeatService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient, private paginationService: PaginationService) {
  }

  getAllSeat(queryParams: QueryParams) {
    let params = this.paginationService
      .getPaginationHeaders(queryParams.pageNumber, queryParams.pageSize);

    if (queryParams.searchTerm) {
      params = params.append('searchTerm', queryParams.searchTerm);
    }

    if (queryParams.sort) {
      params = params.append('sort', queryParams.sort);
    }

    return this.paginationService
      .getPaginatedResult<Seat[]>(this.baseUrl + '/seats', params);
  }

  getAllSeatNoPaging() {
    return this.http.get<Seat[]>(this.baseUrl + '/seats/all');
  }

  getSeatById(id: number) {
    return this.http.get<Seat>(this.baseUrl + '/seats/' + id);
  }

  addSeat(seat: Seat) {
    return this.http.post<Seat>(this.baseUrl + '/seats', seat);
  }

  updateSeat(seat: Seat) {
    return this.http.put<Seat>(this.baseUrl + '/seats/' + seat.id, seat);
  }

  deleteSeat(id: number) {
    return this.http.patch(this.baseUrl + '/seats/' + id, {});
  }
}
