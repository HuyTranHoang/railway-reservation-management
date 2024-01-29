import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Cancellations } from '../core/models/bookingHistory';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  baseUrl = environment.apiUrl

  constructor(private http: HttpClient) { }

  getBookingHistory(userId: string): Observable<any> {
    return this.http.get(this.baseUrl + 'bookinghistory/' + userId)
  }

  canceledTicket(cancellation: Cancellations) {
    return this.http.post(this.baseUrl + 'bookinghistory', cancellation)
  }

  
}
