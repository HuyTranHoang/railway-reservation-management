import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {environment} from '../../../../environments/environment';
import {Payment} from '../../../@models/payment';
import {PaginationService} from '../../shared/pagination.service';
import {PaymentQueryParams} from '../../../@models/params/paymentQueryParams';

@Injectable({
  providedIn: 'root',
})
export class PaymentService {

  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient, private paginationService: PaginationService) {
  }

  getAllPayment(queryParams: PaymentQueryParams) {

    let params = this.paginationService
      .getPaginationHeaders(queryParams.pageNumber, queryParams.pageSize);

    if (queryParams.searchTerm) {
      params = params.append('searchTerm', queryParams.searchTerm);
    }

    if (queryParams.createdAt) {
      params = params.append('createdAt', queryParams.createdAt);
    }

    if (queryParams.sort) {
      params = params.append('sort', queryParams.sort);
    }

    return this.paginationService
      .getPaginatedResult<Payment[]>(this.baseUrl + '/payments', params);

  }

  getPaymentById(id: number) {
    return this.http.get<Payment>(this.baseUrl + '/payments/' + id);
  }

  addPayment(payment: Payment) {
    return this.http.post<Payment>(this.baseUrl + '/payments', payment);
  }

  updatePayment(payment: Payment) {
    return this.http.put<Payment>(this.baseUrl + '/payments/' + payment.id, payment);
  }

  deletePayment(id: number) {
    return this.http.patch(this.baseUrl + '/payments/' + id, {});
  }

}
