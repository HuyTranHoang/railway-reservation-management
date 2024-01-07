import { Component, OnInit } from '@angular/core'
import { AuthService } from './auth/auth.service'

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit{

  constructor(private authService: AuthService) {}
  ngOnInit(): void {
      this.refreshUser()
  }

  private refreshUser() {
    const jwt = this.authService.getJwtToken()

    if (jwt) {
      this.authService.refreshUser(jwt).subscribe({
        next: () => console.log('User refreshed'),
        error: (err) => {
          this.authService.logout()
          console.log(err)
        }
      })
    } else {
      this.authService.refreshUser(null).subscribe()
    }
  }

}
