import { Component, OnInit } from '@angular/core'
import { take } from 'rxjs'
import { AuthService } from 'src/app/auth/auth.service'
import { UserService } from '../user.service'
import { Ticket } from 'src/app/core/models/bookingHistory'
import Swal from 'sweetalert2'
import { User } from '../../core/models/auth/user'


@Component({
  selector: 'app-user-booking-history',
  templateUrl: './user-booking-history.component.html',
  styleUrls: ['./user-booking-history.component.scss']
})
export class UserBookingHistoryComponent implements OnInit {


  currentUser: User | undefined

  upcomingTrips: Ticket[] = []
  pastTrips: Ticket[] = []
  cancellations: Ticket[] = []

  constructor(public authService: AuthService,
              private userService: UserService) {
    this.authService.user$.pipe(take(1)).subscribe({
      next: (user) => {
        if (user) {
          this.currentUser = user
        }
      }
    })
  }

  ngOnInit(): void {
    this.loadBookingHistory()
  }

  loadBookingHistory(isReload = false) {
    if (this.currentUser) {
      this.userService.getBookingHistory(this.currentUser.id).subscribe({
        next: (res) => {
          if (res) {
            this.upcomingTrips = res.upcomingTrips
            this.pastTrips = res.pastTrips
            this.cancellations = res.cancellations

            if (isReload) {
              Swal.fire({
                title: 'Ticket Canceled',
                text: 'Your ticket has been canceled successfully.',
                icon: 'success',
                confirmButtonText: 'Ok'
              })
            }
          }
        }
      })
    }
  }

  onConfirmCancel() {
    this.loadBookingHistory(true)
  }

}

