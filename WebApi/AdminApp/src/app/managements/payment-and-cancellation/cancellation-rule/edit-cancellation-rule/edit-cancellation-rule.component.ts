import {Component, OnInit} from '@angular/core';
import {AbstractControl, FormBuilder, FormGroup, ValidationErrors, ValidatorFn, Validators} from '@angular/forms';
import {CancellationRuleService} from '../cancellation-rule.service';
import {NbGlobalPhysicalPosition, NbToastrService} from '@nebular/theme';
import {ActivatedRoute, Router} from '@angular/router';

@Component({
  selector: 'ngx-edit-cancellation-rule',
  templateUrl: './edit-cancellation-rule.component.html',
  styleUrls: ['./edit-cancellation-rule.component.scss'],
})
export class EditCancellationRuleComponent implements OnInit {
  cancellationRuleForm: FormGroup = this.fb.group({});

  isSubmitted = false;
  errorMessages: string[] = [];

  isLoading = false;

  constructor(private cancellationRuleService: CancellationRuleService,
              private toastrService: NbToastrService,
              private activatedRoute: ActivatedRoute,
              private router: Router,
              private fb: FormBuilder) {
  }

  ngOnInit(): void {
    this.initForm();
  }

  initForm() {
    this.cancellationRuleForm = this.fb.group({
      id: ['', Validators.required],
      departureDateDifference: ['', [Validators.required, Validators.min(0), this.numberValidator()]],
      fee: ['', [Validators.required, Validators.min(0), this.numberValidator()]],
      status: [''],
    });

    const id = this.activatedRoute.snapshot.params.id;
    this.isLoading = true;
    this.cancellationRuleService.getCancellationRuleById(id)
      .subscribe({
        next: (res) => {
          this.cancellationRuleForm.patchValue(res);
          this.isLoading = false;
        },
        error: (err) => {
          this.showToast('danger', 'Failed', 'Cancellation Rule doest not exist!');
          this.router.navigateByUrl('/managements/payment-and-cancellation/cancellation-rule');
        },
      });

  }

  numberValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      if (control.value == null || control.value === '') return null;
      return !isNaN(parseFloat(control.value)) && isFinite(control.value) ? null : {'notANumber': true};
    };
  }

  onSubmit() {
    this.isSubmitted = true;

    if (this.cancellationRuleForm.valid) {
      this.cancellationRuleService.updateCancellationRule(this.cancellationRuleForm.value).subscribe({
        next: (res) => {
          this.showToast('success', 'Success', 'Update cancellation rule successfully!');
          this.isSubmitted = false;
          this.errorMessages = [];
        },
        error: (err) => {
          this.showToast('danger', 'Failed', 'Update cancellation rule failed!');
          this.errorMessages = err.error.errors;
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
