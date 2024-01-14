import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Compartment } from '../../../@models/compartment';
import { CompartmentQueryParams } from '../../../@models/params/compartmentQueryParams';
import { PaginationService } from '../../shared/pagination.service';

@Injectable({
  providedIn: 'root'
})
export class CompartmentService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient, private paginationService: PaginationService) {
  }

  getAllCompartments(queryParams: CompartmentQueryParams) {

    let params = this.paginationService
      .getPaginationHeaders(queryParams.pageNumber, queryParams.pageSize);

    if (queryParams.searchTerm) {
      params = params.append('searchTerm', queryParams.searchTerm);
    }

    if (queryParams.sort) {
      params = params.append('sort', queryParams.sort);
    }

    if (queryParams.carriageId) {
      params = params.append('carriageId', queryParams.carriageId);
    }

    return this.paginationService
      .getPaginatedResult<Compartment[]>(this.baseUrl + '/compartments', params);

  }

  getAllCompartmentNoPaging() {
    return this.http.get<Compartment>(this.baseUrl + '/compartments/all');
  }

  getCompartmentById(id: number) {
    return this.http.get<Compartment>(this.baseUrl + '/compartments/' + id);
  }

  addCompartment(compartment: Compartment) {
    return this.http.post<Compartment>(this.baseUrl + '/compartments', compartment);
  }

  updateCompartment(compartment: Compartment) {
    return this.http.put<Compartment>(this.baseUrl + '/compartments/' + compartment.id, compartment);
  }

  deleteCompartment(id: number) {
    return this.http.patch(this.baseUrl + '/compartments/' + id, {});
  }
}
