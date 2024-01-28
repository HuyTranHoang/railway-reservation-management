import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  baseUrl = environment.apiUrl

  constructor(private http: HttpClient) { }

  getBookingHistory(userId: string): Observable<any> {
    return this.http.get(this.baseUrl + 'bookinghistory/' + userId)
  }
}
