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
        if (res) {
          this.manageService.currentTicket = res
          this.router.navigate(['management/ticket'])
        }
      },
      error: (error: any) => {
        this.wrong = 'Email or ticket code is wrong!'
      }
    })
  }

}
