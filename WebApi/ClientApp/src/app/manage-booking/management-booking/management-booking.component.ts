import { Component, OnInit } from '@angular/core'
import { ManageBookingService } from '../manage-booking.service'
import { Router } from '@angular/router'
import { FormBuilder, FormGroup, Validators } from '@angular/forms'


@Component({
  selector: 'app-management-booking',
  templateUrl: './management-booking.component.html',
  styleUrls: ['./management-booking.component.scss']
})
export class ManagementBookingComponent implements OnInit {

  lookUpForm: FormGroup = this.fb.group({})

  isSubmitted = false
  errorMessages: string[] = []

  constructor(private manageService: ManageBookingService,
              private fb: FormBuilder,
              private router: Router) {
  }

  ngOnInit(): void {
    this.initForm()
    this.manageService.currentTicket = undefined
  }

  initForm() {
    this.lookUpForm = this.fb.group({
      email: ['', Validators.required],
      code: ['', Validators.required]
    })
  }

  onSubmit() {
    console.log(this.lookUpForm.value)
    this.isSubmitted = true

    if (this.lookUpForm.invalid) {
      return
    }

    const email = this.lookUpForm.value.email
    const code = this.lookUpForm.value.code

    this.manageService.getLookUpByEmailAndCode(code, email).subscribe({
      next: (res) => {
        console.log(res)
        if (res) {
          this.manageService.currentTicket = res
          this.router.navigate(['management/ticket'])
          this.isSubmitted = false
          this.errorMessages = []
        }
      },
      error: (error: any) => {
        this.errorMessages = error.errors
      }
    })
  }

}
