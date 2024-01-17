import {Injectable} from '@angular/core';
import {environment} from '../../../../environments/environment';
import {HttpClient} from '@angular/common/http';
import {PaginationService} from '../../shared/pagination.service';
import {QueryParams} from '../../../@models/params/queryParams';
import {DistanceFare} from '../../../@models/distanceFare';

@Injectable({
  providedIn: 'root',
})
export class DistanceFareService {

  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient, private paginationService: PaginationService) {
  }

  getAllDistanceFares(queryParams: QueryParams) {
    let params = this.paginationService.getPaginationHeaders(queryParams.pageNumber, queryParams.pageSize);

    if (queryParams.searchTerm) {
      params = params.append('searchTerm', queryParams.searchTerm);
    }

    if (queryParams.sort) {
      params = params.append('sort', queryParams.sort);
    }

    return this.paginationService.getPaginatedResult<DistanceFare[]>(this.baseUrl + '/distancefares', params);
  }

  getDistanceFareById(id: number) {
    return this.http.get<DistanceFare>(this.baseUrl + '/distancefares/' + id);
  }

  addDistanceFare(distanceFare: DistanceFare) {
    return this.http.post<DistanceFare>(this.baseUrl + '/distancefares', distanceFare);
  }

  updateDistanceFare(distanceFare: DistanceFare) {
    return this.http.put<DistanceFare>(this.baseUrl + '/distancefares/' + distanceFare.id, distanceFare);
  }

  deleteDistanceFare(id: number) {
    return this.http.patch(this.baseUrl + '/distancefares/' + id, {});
  }

  getTrainCompanyById(id: number) {
    return this.http.get<DistanceFare>(this.baseUrl + '/traincompanies/' + id);

  }

}
