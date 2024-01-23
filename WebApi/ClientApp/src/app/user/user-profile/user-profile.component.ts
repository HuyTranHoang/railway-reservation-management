import { Component, OnInit } from '@angular/core'
import { AuthService } from '../../auth/auth.service'
import { AbstractControl, FormBuilder, FormGroup, ValidationErrors, Validators } from '@angular/forms'
import { ChangePassword } from '../../core/models/auth/changePassword'
import Swal from 'sweetalert2'

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.scss']
})
export class UserProfileComponent implements OnInit {

  isSubmitted = false
  errorMessages = []

  profileForm: FormGroup = new FormGroup({})
  passwordForm: FormGroup = new FormGroup({})

  constructor(public authService: AuthService, private fb: FormBuilder) {}

  ngOnInit(): void {
    this.initializeForm()

    this.passwordForm.get('newPassword')?.valueChanges.subscribe(() => {
      this.passwordForm.get('confirmPassword')?.updateValueAndValidity();
    });
  }

  initializeForm(): void {
    this.passwordForm = this.fb.group({
      currentPassword: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(15)]],
      newPassword: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(15)]],
      confirmPassword: ['', [Validators.required, this.passwordMatchValidator.bind(this)]]
    })

    this.profileForm = this.fb.group({
      firstName: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(15)]],
      lastName: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(15)]],
      phoneNumber: ['']
    })
  }

  passwordMatchValidator(control: AbstractControl): ValidationErrors | null {
    const password = this.passwordForm.get('newPassword')?.value
    if (control.value !== password) {
      return { 'mismatch': true }
    }
    return null
  }

  onChangePasswordSubmit() {
    this.isSubmitted = true
    this.errorMessages = []

    if (this.passwordForm.invalid) {
      return
    }

    const formData: ChangePassword = {
      currentPassword: this.passwordForm.value.currentPassword,
      newPassword: this.passwordForm.value.newPassword
    }

    this.authService.changePassword(formData).subscribe({
      next: _ => {
        Swal.fire({
          title: 'Password Changed!',
          text: 'Your password has been changed successfully.',
          icon: 'success',
          confirmButtonText: 'Ok'
        })
        this.isSubmitted = false
        this.errorMessages = []
        this.passwordForm.reset()
      },
      error: (err) => {
        this.isSubmitted = false
        this.errorMessages = err
      }
    })
  }

  onProfileSubmit() {
    this.isSubmitted = true
    this.errorMessages = []

    if (this.profileForm.invalid) {
      return
    }

    this.authService.updateProfile(this.profileForm.value).subscribe({
      next: _ => {
        Swal.fire({
          title: 'Profile Updated!',
          text: 'Your profile has been updated successfully.',
          icon: 'success',
          confirmButtonText: 'Ok'
        })
        this.isSubmitted = false
        this.errorMessages = []
      },
      error: (err) => {
        this.isSubmitted = false
        this.errorMessages = err.errors
      }
    })
  }

}
