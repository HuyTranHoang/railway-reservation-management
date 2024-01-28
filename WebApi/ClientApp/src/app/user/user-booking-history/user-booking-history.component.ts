import { Component, OnInit, ViewChild } from '@angular/core';
import { TabsetComponent } from 'ngx-bootstrap/tabs';
import { take } from 'rxjs';
import { AuthService } from 'src/app/auth/auth.service';
import { UserService } from '../user.service';
import { BookingHistory, Cancellations, PastTrips, UpcomingTrip } from 'src/app/core/models/bookingHistory';
import { Router } from '@angular/router';

@Component({
  selector: 'app-user-booking-history',
  templateUrl: './user-booking-history.component.html',
  styleUrls: ['./user-booking-history.component.scss']
})
export class UserBookingHistoryComponent implements OnInit {

  upcomingTrips: UpcomingTrip[] = [];
  pastTrips: PastTrips[] = [];
  cancellations: Cancellations[] = [];

  constructor(public authService : AuthService,
              private userService : UserService,
              private router: Router){
  }

  ngOnInit(): void {
    this.loadBookingHistory();
  }

  loadBookingHistory(){
    this.authService.user$.pipe(take(1)).subscribe({
      next: (user) => {
        if (user) {
          this.userService.getBookingHistory(user.id).subscribe({
            next: (res) => {
              if (res) {
                this.upcomingTrips = res['upcomingTrips'];
                this.pastTrips = res['pastTrips'];
                this.cancellations = res['cancellations'];
              }
            },
          });
        } else {
          this.router.navigateByUrl('/');
        }
      },
    });
  }
}
