import {Component, OnInit} from '@angular/core';
import {AbstractControl, FormBuilder, FormGroup, ValidationErrors, ValidatorFn, Validators} from '@angular/forms';
import { CancellationRuleService } from '../cancellation-rule.service';
import {NbGlobalPhysicalPosition, NbToastrService} from '@nebular/theme';
import {ActivatedRoute, Router} from '@angular/router';

@Component({
  selector: 'ngx-edit-cancellation-rule',
  templateUrl: './edit-cancellation-rule.component.html',
  styleUrls: ['./edit-cancellation-rule.component.scss']
})
export class EditCancellationRuleComponent  implements OnInit{
  cancellationRuleForm: FormGroup = this.fb.group({});

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
    const id = this.activatedRoute.snapshot.params.id;

    this.cancellationRuleService.getCancellationRuleById(id)
      .subscribe({
        next: (res) => {
          this.cancellationRuleForm = this.fb.group({
            id: [res.id, Validators.required],
            departureDateDifference: [res.departureDateDifference, [Validators.required, Validators.min(0), this.numberValidator()]],
            fee: [res.fee, [Validators.required, Validators.min(0), this.numberValidator()]],
            status: [res.status],
          });
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
      return !isNaN(parseFloat(control.value)) && isFinite(control.value) ? null : { 'notANumber': true };
    };
  }

  onSubmit() {
    if (this.cancellationRuleForm.valid) {
      this.cancellationRuleService.updateCancellationRule(this.cancellationRuleForm.value).subscribe({
        next: (res) => {
          this.showToast('success', 'Success', 'Update cancellation rule successfully!');
        },
        error: (err) => {
          this.showToast('danger', 'Failed', 'Update cancellation rule failed!');
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
