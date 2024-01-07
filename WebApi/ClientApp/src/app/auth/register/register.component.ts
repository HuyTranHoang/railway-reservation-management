import { Component, OnInit } from '@angular/core'
import { AuthService } from '../auth.service'
import { AbstractControl, FormBuilder, FormGroup, ValidationErrors, Validators } from '@angular/forms'
import Swal from 'sweetalert2'
import { Router } from '@angular/router'

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['../login-and-register.component.scss']
})
export class RegisterComponent implements OnInit {
  registerForm: FormGroup = new FormGroup({})
  submitted = false
  errorMessages: string[] = []

  constructor(private authService: AuthService, private fb: FormBuilder, private router: Router) { }

  ngOnInit(): void {
    this.initializeForm()
  }

  initializeForm(): void {
    this.registerForm = this.fb.group({
      firstName: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(15)]],
      lastName: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(15)]],
      email: ['', [Validators.required, Validators.pattern('^\\w+@[a-zA-Z_]+?\\.[a-zA-Z]{2,3}$')]],
      password: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(15)]],
      confirmPassword: ['', [Validators.required, this.passwordMatchValidator.bind(this)]]
    });
  }

  passwordMatchValidator(control: AbstractControl): ValidationErrors | null {
    const password = this.registerForm.get('password')?.value;
    if (control.value !== password) {
      return { 'mismatch': true };
    }
    return null;
  }

  onSubmit() {
    this.submitted = true
    this.errorMessages = []

    if (this.registerForm.invalid) {
      return
    }
    
    const formData = {
      firstName: this.registerForm.value.firstName,
      lastName: this.registerForm.value.lastName,
      email: this.registerForm.value.email,
      password: this.registerForm.value.password
    };

    this.authService.register(formData).subscribe({
      next: (res: any) => {
        Swal.fire({
          position: 'center',
          icon: 'success',
          title: res.value.title,
          text: res.value.message,
          showConfirmButton: false,
          timer: 2000
        }).then(() => {
          this.router.navigate(['auth//login'])
        })

        this.errorMessages = []
        this.submitted = false
        this.registerForm.reset()
      },
      error: (err) => {
        console.log(err.errors)
        this.errorMessages = err.errors
      }
    })
  }

}
