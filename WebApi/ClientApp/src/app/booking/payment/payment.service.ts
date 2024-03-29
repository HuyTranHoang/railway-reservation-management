import { Injectable } from '@angular/core'
import { environment } from '../../../environments/environment.development'
import { HttpClient } from '@angular/common/http'
import { PaymentInformation } from '../../core/models/paymentInformation'
import { BehaviorSubject, Observable } from 'rxjs'
import { RoundTrip } from '../../core/models/roundTrip'

@Injectable({
  providedIn: 'root'
})
export class PaymentService {

  baseUrl = environment.apiUrl

  private paymentStatusSource = new BehaviorSubject<string | null>(null)
  paymentStatus$ = this.paymentStatusSource.asObservable()

  constructor(private http: HttpClient) { }

  addPaymentByEmail(email: string, transactionId: string) {
    return this.http.post(this.baseUrl + 'payments/addPaymentByEmail/' + email + '/' + transactionId, {})
  }

  createPaymentUrl(paymentInfo: PaymentInformation): Observable<any> {
    return this.http.post(this.baseUrl + 'payments/createUrlVnPay', paymentInfo)
  }

  setPaymentStatus(status: string): void {
    this.paymentStatusSource.next(status)
  }

  getRoundTripByTrainCompanyId(trainCompanyId: number) {
    return this.http.get<RoundTrip>(this.baseUrl + 'roundTrip/trainCompany/' + trainCompanyId)
  }
}
