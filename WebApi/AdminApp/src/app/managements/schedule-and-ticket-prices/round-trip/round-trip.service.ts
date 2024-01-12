import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {environment} from '../../../../environments/environment';
import {RoundTrip} from '../../../@models/roundTrip';
import {QueryParams} from '../../../@models/params/queryParams';
import {PaginationService} from '../../shared/pagination.service';

@Injectable({
  providedIn: 'root',
})
export class RoundTripService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient, private paginationService: PaginationService) {
  }

  getAllRoundTrip(queryParams: QueryParams) {
    let params = this.paginationService.getPaginationHeaders(queryParams.pageNumber, queryParams.pageSize);

    if (queryParams.searchTerm) {
      params = params.append('searchTerm', queryParams.searchTerm);
    }

    if (queryParams.sort) {
      params = params.append('sort', queryParams.sort);
    }

    return this.paginationService.getPaginatedResult<RoundTrip[]>(this.baseUrl + '/roundtrip', params);
  }

  addRoundTrip(roundTrip: RoundTrip) {
    return this.http.post<RoundTrip>(this.baseUrl + '/roundtrip', roundTrip);
  }

  getRoundTripById(id: number) {
    return this.http.get<RoundTrip>(this.baseUrl + '/roundtrip/' + id);
  }

  updateRoundTrip(roundTrip: RoundTrip) {
    return this.http.put<RoundTrip>(this.baseUrl + '/roundtrip/' + roundTrip.id, roundTrip);
  }

  deleteRoundTrip(id: number) {
    return this.http.patch(this.baseUrl + '/roundtrip/' + id, {});
  }
}
