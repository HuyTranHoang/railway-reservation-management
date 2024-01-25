import { Component } from '@angular/core'
import { ManageBookingService } from '../manage-booking.service'
import { Router } from '@angular/router'


@Component({
  selector: 'app-management-booking',
  templateUrl: './management-booking.component.html',
  styleUrls: ['./management-booking.component.scss']
})
export class ManagementBookingComponent {
  code: string = ''
  email: string = ''
  wrong: string = ''


  constructor(private manageService: ManageBookingService,
              private router: Router
  ) {
  }

  onSubmit() {
    this.manageService.getLookUpByEmailAndCode(this.code, this.email).subscribe({
      next: (res) => {
        this.router.navigate(['management/ticket'], {
          queryParams: {
            email: this.email,
            code: this.code,
            passengerName: res.passengerName,
            trainName: res.trainName,
            carriageName: res.carriageName,
            seatName: res.seatName,
            scheduleName: res.scheduleName,
            price: res.price,
            scheduleId: res.scheduleId
          }
        })
      },

      error: (error: any) => {
        console.log(error)
        this.router.navigateByUrl('management')
        this.wrong = 'Failed wrong Email or Code please try again !'
      }
    })
  }

}
