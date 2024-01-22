import {Component, EventEmitter, Input, Output} from '@angular/core';
import {NbDialogRef, NbGlobalPhysicalPosition, NbToastrService} from '@nebular/theme';
import {UserService} from '../user.service';

@Component({
  selector: 'ngx-confirm-lock-user',
  templateUrl: './confirm-lock-user.component.html',
  styleUrls: ['./confirm-lock-user.component.scss'],
})
export class ConfirmLockUserComponent {

  @Input() id: string;
  @Input() fullName: string;
  @Input() isLocked: boolean;
  @Output() onConfirmLock = new EventEmitter<void>();

  constructor(protected ref: NbDialogRef<ConfirmLockUserComponent>,
              private userService: UserService,
              private toastrService: NbToastrService) {
  }

  dismiss() {
    this.ref.close();
  }

  onLock() {
    if (!this.isLocked) {
      this.userService.lockUser(this.id).subscribe({
        next: _ => {
          this.showToast('success', 'Success', 'Lock user successfully!');
          this.onConfirmLock.emit();
          this.ref.close();
        },
        error: _ => {
          this.showToast('danger', 'Failed', 'Lock user failed!');
        },
      });
    } else {
      this.userService.unlockUser(this.id).subscribe({
        next: _ => {
          this.showToast('success', 'Success', 'Unlock user successfully!');
          this.onConfirmLock.emit();
          this.ref.close();
        },
        error: _ => {
          this.showToast('danger', 'Failed', 'Unlock user failed!');
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
