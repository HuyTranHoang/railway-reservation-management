import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {environment} from '../../../../environments/environment';
import {Ticket} from '../../../@models/ticket';
import {PaginationService} from '../../shared/pagination.service';
import {TicketQueryParams} from '../../../@models/params/ticketQueryParams';

@Injectable({
  providedIn: 'root',
})
export class TicketService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient, private paginationService: PaginationService) {
  }

  getAllTicket(queryParams: TicketQueryParams) {
    let params = this.paginationService
      .getPaginationHeaders(queryParams.pageNumber, queryParams.pageSize);

    if (queryParams.passengerId) {
      params = params.append('passengerId', queryParams.passengerId.toString());
    }

    if (queryParams.trainId) {
      params = params.append('trainId', queryParams.trainId.toString());
    }

    if (queryParams.distanceFareId) {
      params = params.append('distanceFareId', queryParams.distanceFareId.toString());
    }

    if (queryParams.carriageId) {
      params = params.append('carriageId', queryParams.carriageId.toString());
    }

    if (queryParams.seatId) {
      params = params.append('seatId', queryParams.seatId.toString());
    }

    if (queryParams.scheduleId) {
      params = params.append('scheduleId', queryParams.scheduleId.toString());
    }

    if (queryParams.scheduleId) {
      params = params.append('scheduleId', queryParams.scheduleId.toString());
    }

    if (queryParams.paymentId) {
      params = params.append('paymentId', queryParams.paymentId.toString());
    }

    if (queryParams.createdAt) {
      params = params.append('createdAt', queryParams.createdAt.toString());
    }

    if (queryParams.searchTerm) {
      params = params.append('searchTerm', queryParams.searchTerm);
    }

    if (queryParams.sort) {
      params = params.append('sort', queryParams.sort);
    }

    return this.paginationService
      .getPaginatedResult<Ticket[]>(this.baseUrl + '/tickets', params);
  }

  getAllTicketNoPaging() {
    return this.http.get<Ticket[]>(this.baseUrl + '/tickets/all');
  }

  getTicketById(id: number) {
    return this.http.get<Ticket>(this.baseUrl + '/tickets/' + id);
  }

  addTicket(ticket: Ticket) {
    return this.http.post<Ticket>(this.baseUrl + '/tickets', ticket);
  }

  updateTicket(ticket: Ticket) {
    return this.http.put<Ticket>(this.baseUrl + '/tickets/' + ticket.id, ticket);
  }

  deleteTicket(id: number) {
    return this.http.patch(this.baseUrl + '/Tickets/' + id, {});
  }
}
