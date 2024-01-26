import { Injectable } from '@angular/core'
import { environment } from '../../../environments/environment.development'
import { HttpClient } from '@angular/common/http'
import { PaymentInformation } from '../../core/models/paymentInformation'

@Injectable({
  providedIn: 'root'
})
export class PaymentService {

  baseUrl = environment.apiUrl

  constructor(private http: HttpClient) { }

  createPaymentUrl(paymentInfo: PaymentInformation) {
    return this.http.post(this.baseUrl + 'payments/createUrlVnPay', paymentInfo);
  }
}
