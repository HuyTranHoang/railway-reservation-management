import { Component } from '@angular/core';
import { ManageBookingService } from '../manage-booking.service';
import {  Router } from '@angular/router';


@Component({
  selector: 'app-management-booking',
  templateUrl: './management-booking.component.html',
  styleUrls: ['./management-booking.component.scss']
})
export class ManagementBookingComponent {
  code: string = '';
  email: string = '';
  wrong : string = "";

  
  constructor(private manageService : ManageBookingService,
    private router: Router,
    
    ) {
  }
  onSubmit() {
    this.manageService.getLookUpByEmailAndCode(this.code , this.email).subscribe({
      next: (respon) => {
        console.log(respon);
        this.router.navigateByUrl('management/ticket');
        this.router.navigate(['management/ticket'], { queryParams: {
           email: this.email,
           code: this.code,
           passengerName : respon.passengerName,
           trainName : respon.trainName,
           carriageName : respon.carriageName,
           seatName : respon.seatName,
           scheduleName : respon.scheduleName,
           price : respon.price,
           scheduleId : respon.scheduleId,
          } });
      },
      error: (error: any) => {
        console.log(error);
        this.router.navigateByUrl('management');
        this.wrong = "Failed wrong Email or Code please try again !"
      }
    });               
  }

}
