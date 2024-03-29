import {Injectable} from '@angular/core';
import {environment} from '../../../environments/environment';
import {HttpClient} from '@angular/common/http';
import {PaginationService} from '../shared/pagination.service';
import {QueryParams} from '../../@models/params/queryParams';
import {User} from '../../@models/auth/applicationUser';

@Injectable({
  providedIn: 'root',
})
export class UserService {

  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient, private paginationService: PaginationService) {
  }

  getAllUser(queryParams: QueryParams) {
    let params = this.paginationService
      .getPaginationHeaders(queryParams.pageNumber, queryParams.pageSize);

    if (queryParams.searchTerm) {
      params = params.append('searchTerm', queryParams.searchTerm);
    }

    if (queryParams.sort) {
      params = params.append('sort', queryParams.sort);
    }

    return this.paginationService
      .getPaginatedResult<User[]>(this.baseUrl + '/users', params);
  }

  getUserByEmail(email: string) {
    return this.http.get<User>(this.baseUrl + '/users/' + email);
  }

  getUserById(id: string) {
    return this.http.get<User>(this.baseUrl + '/users/get-user-by-id/' + id);
  }

  lockUser(id: string) {
    return this.http.put(this.baseUrl + '/users/lock-user/' + id, {});
  }

  unlockUser(id: string) {
    return this.http.put(this.baseUrl + '/users/unlock-user/' + id, {});
  }

  getAllRoles() {
    return this.http.get<string[]>(this.baseUrl + '/users/get-application-roles');
  }

  addOrEditUser(user: User) {
    return this.http.post(this.baseUrl + '/users/add-edit-user', user);
  }
}
