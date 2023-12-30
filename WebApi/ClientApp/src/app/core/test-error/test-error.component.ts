import { Component } from '@angular/core';
import { environment } from '../../../environments/environment.development'
import { HttpClient } from '@angular/common/http'

@Component({
  selector: 'app-test-error',
  templateUrl: './test-error.component.html',
  styleUrls: ['./test-error.component.scss']
})
export class TestErrorComponent {
  baseUrl = environment.apiUrl
  validationErrors: string[] = []

  constructor(private http: HttpClient) { }

  get404Error() {
    this.http.get(this.baseUrl + 'buggy/end-point-does-not-exist').subscribe({
      next: response => console.log(response),
      error: err => console.log(err)
    })
  }

  get500Error() {
    this.http.get(this.baseUrl + 'buggy/server-error').subscribe({
      next: response => console.log(response),
      error: err => console.log(err)
    })
  }

  get400Error() {
    this.http.get(this.baseUrl + 'buggy/bad-request').subscribe({
      next: response => console.log(response),
      error: err => console.log(err)
    })
  }

  get400ValidationError() {
    this.http.get(this.baseUrl + 'buggy/bad-request/one').subscribe({
      next: response => console.log(response),
      error: err => {
        console.log(err)
        this.validationErrors = err.errors
      }
    })
  }

  get401Error() {
    this.http.get(this.baseUrl + 'buggy/auth').subscribe({
      next: response => console.log(response),
      error: err => console.log(err)
    })
  }

}
