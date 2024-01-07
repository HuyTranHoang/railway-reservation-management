import { Component, OnInit } from '@angular/core'
import { FormBuilder, FormGroup, Validators } from '@angular/forms'
import { AuthService } from '../auth.service'
import { Router } from '@angular/router'
import Swal from 'sweetalert2'

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['../login-and-register.component.scss']
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup = new FormGroup({})
  submitted = false
  errorMessages: string[] = []

  constructor(private authService: AuthService, private fb: FormBuilder, private router: Router) {}

  ngOnInit(): void {
    this.initializeForm()
  }

  initializeForm(): void {
    this.loginForm = this.fb.group({
      userName: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  onSubmit() {
    this.submitted = true
    this.errorMessages = []

    if (this.loginForm.invalid) {
      return
    }
    this.authService.login(this.loginForm.value).subscribe({
      next: (res: any) => {
        Swal.fire({
          position: 'center',
          icon: 'success',
          title: 'You have successfully logged in',
          showConfirmButton: false,
          timer: 1500
        })
        localStorage.setItem('token', res.token)
        this.router.navigateByUrl('/')
      },
      error: (err) => {
        console.log(err)
      }
    })
  }
}
