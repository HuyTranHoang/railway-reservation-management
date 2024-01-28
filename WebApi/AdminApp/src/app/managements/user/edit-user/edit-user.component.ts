import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {UserService} from '../user.service';
import {NbGlobalPhysicalPosition, NbToastrService} from '@nebular/theme';
import {ActivatedRoute, Router} from '@angular/router';

@Component({
  selector: 'ngx-edit-user',
  templateUrl: './edit-user.component.html',
  styleUrls: ['./edit-user.component.scss'],
})
export class EditUserComponent implements OnInit {

  userForm: FormGroup = this.fb.group({});
  isSubmitted = false;
  errorMessages: string[] = [];

  roles = [];

  isLoading = false;

  constructor(private userService: UserService,
              private toastrService: NbToastrService,
              private activatedRoute: ActivatedRoute,
              private router: Router,
              private fb: FormBuilder) {

  }

  ngOnInit(): void {
    this.loadRoles();
    this.initForm();
  }

  private loadRoles() {
    this.userService.getAllRoles().subscribe({
      next: (roles) => {
        this.roles = roles;
      },
    });
  }

  initForm() {

    this.userForm = this.fb.group({
      id: ['', Validators.required],
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      userName: ['', Validators.required],
      password: ['', Validators.required],
      roles: ['', Validators.required],
    });

    const id = this.activatedRoute.snapshot.params.id;
    this.isLoading = true;

    this.userService.getUserById(id).subscribe({
      next: (res) => {
        this.userForm.patchValue(res);
        this.isLoading = false;
      },
      error: (err) => {
        this.showToast('danger', 'Failed', 'User doest not exist!');
        this.router.navigateByUrl('/managements/user');
      },
    });

  }

  onSubmit() {
    this.isSubmitted = true;

    if (this.userForm.valid) {
      this.userService.addOrEditUser(this.userForm.value).subscribe({
        next: _ => {
          this.showToast('success', 'Success', 'Add user successfully!');
          this.userForm.reset();
          this.isSubmitted = false;
          this.errorMessages = [];
        },
        error: (err) => {
          this.showToast('danger', 'Failed', 'Add user failed!');
          if (err.error.errors) {
            this.errorMessages = err.error.errors;
          } else {
            this.errorMessages = err.error.errorMessages;
          }
        },
      });
    }
  }

  private showToast(type: string, title: string, body: string) {
    const config = {
      status: type,
      destroyByClick: true,
      duration: 2000,
      hasIcon: true,
      position: NbGlobalPhysicalPosition.TOP_RIGHT,
    };
    this.toastrService.show(
      body,
      title,
      config);
  }

}
