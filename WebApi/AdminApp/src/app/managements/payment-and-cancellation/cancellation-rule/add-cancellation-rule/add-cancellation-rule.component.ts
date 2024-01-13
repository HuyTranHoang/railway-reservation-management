import {Component, OnInit} from '@angular/core';
import { CancellationRuleService } from '../cancellation-rule.service';
import {AbstractControl, FormBuilder, FormGroup, ValidationErrors, ValidatorFn, Validators} from '@angular/forms';
import {NbComponentStatus, NbGlobalPhysicalPosition, NbToastrService} from '@nebular/theme';

@Component({
  selector: 'ngx-add-cancellation-rule',
  templateUrl: './add-cancellation-rule.component.html',
  styleUrls: ['./add-cancellation-rule.component.scss']
})
export class AddCancellationRuleComponent implements OnInit {
  cancellationRuleForm: FormGroup = this.fb.group({});

  isSubmitted = false;
  errorMessages: string[] = [];

  constructor(
    private cancellationRuleService: CancellationRuleService,
    private fb: FormBuilder,
    private toastrService: NbToastrService,
  ) {
  }

  ngOnInit(): void {
    this.initForm();
  }

  initForm() {
    this.cancellationRuleForm = this.fb.group({
      departureDateDifference: [0, [Validators.required, Validators.min(0), this.numberValidator()]],
      fee: [0, [Validators.required, Validators.min(0), this.numberValidator()]],
      status: [''],
    });
  }

  numberValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      if (control.value == null || control.value === '') return null;
      return !isNaN(parseFloat(control.value)) && isFinite(control.value) ? null : { 'notANumber': true };
    };
  }

  onSubmit() {
    this.isSubmitted = true;
    if (this.cancellationRuleForm.valid) {
      this.cancellationRuleService.addCancellationRule(this.cancellationRuleForm.value).subscribe({
        next: (res) => {
          this.showToast('success', 'Success', 'Add cancellation rule successfully!');
          this.cancellationRuleForm.reset();
          this.isSubmitted = false;
          this.errorMessages = [];
        },
        error: (err) => {
          this.showToast('danger', 'Failed', 'Add cancellation rule failed!');
          this.errorMessages = err.error.errorMessages;
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
