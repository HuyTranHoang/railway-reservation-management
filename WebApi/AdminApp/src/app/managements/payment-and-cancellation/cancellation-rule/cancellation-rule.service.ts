import {Injectable} from '@angular/core';
import {environment} from '../../../../environments/environment';
import {HttpClient} from '@angular/common/http';
import {PaginationService} from '../../shared/pagination.service';
import {QueryParams} from '../../../@models/params/queryParams';
import {CancellationRule} from '../../../@models/cancellationRule';

@Injectable({
  providedIn: 'root'
})
export class CancellationRuleService {

  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient, private paginationService: PaginationService) {
  }

  getAllCancellationRule(queryParams: QueryParams) {
    let params = this.paginationService.getPaginationHeaders(queryParams.pageNumber, queryParams.pageSize);

    if (queryParams.searchTerm) {
      params = params.append('searchTerm', queryParams.searchTerm);
    }

    if (queryParams.sort) {
      params = params.append('sort', queryParams.sort);
    }

    return this.paginationService.getPaginatedResult<CancellationRule[]>(this.baseUrl + '/cancellationRules', params);
  }

  getAllCancellationRuleNoPaging() {
    return this.http.get<CancellationRule[]>(this.baseUrl + '/cancellationRules/all');
  }

  addCancellationRule(cancellationRule: CancellationRule) {
    return this.http.post<CancellationRule>(this.baseUrl + '/cancellationRules', cancellationRule);
  }

  getCancellationRuleById(id: number) {
    return this.http.get<CancellationRule>(this.baseUrl + '/cancellationRules/' + id);
  }

  updateCancellationRule(cancellationRule: CancellationRule) {
    return this.http.put<CancellationRule>(this.baseUrl + '/cancellationRules/' + cancellationRule.id, cancellationRule);
  }

  deleteCancellationRule(id: number) {
    return this.http.patch(this.baseUrl + '/cancellationRules/' + id, {});
  }
}
