import {CompartmentService} from '../compartment.service';
import {Component, OnInit, ViewChild} from '@angular/core';
import {AbstractControl, FormBuilder, FormGroup, ValidationErrors, ValidatorFn, Validators} from '@angular/forms';
import {NbGlobalPhysicalPosition, NbToastrService} from '@nebular/theme';
import {CarriageService} from '../../carriage/carriage.service';
import {Carriage} from '../../../../@models/carriage';
import {map} from 'rxjs/operators';
import {Observable, of} from 'rxjs';

@Component({
  selector: 'ngx-add-compartment',
  templateUrl: './add-compartment.component.html',
  styleUrls: ['./add-compartment.component.scss']
})
export class AddCompartmentComponent implements OnInit {
  options: Carriage[];
  filteredOptions$: Observable<Carriage[]>;
  isCarriageSelected: boolean = false;

  @ViewChild('autoInput') input: { nativeElement: { value: string; }; };

  compartmentForm: FormGroup = this.fb.group({});
  isSubmitted: boolean = false;
  errorMessages: string[] = [];

  constructor(private compartmentService: CompartmentService,
              private carriageService: CarriageService,
              private toastrService: NbToastrService,
              private fb: FormBuilder) {
  }

  ngOnInit(): void {
    this.initForm();
    this.loadAllCarriage();
  }

  initForm() {
    this.compartmentForm = this.fb.group({
      name: ['', Validators.required],
      carriageId: ['', Validators.required],
      numberOfSeats: [0, [Validators.required, Validators.min(0), this.numberValidator()]],
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
    this.errorMessages = [];

    if (!this.isCarriageSelected && this.input.nativeElement.value !== '') {
      this.errorMessages.push('Please select a valid carriage.');
      return;
    }

    if (this.compartmentForm.valid) {
      this.compartmentService.addCompartment(this.compartmentForm.value).subscribe({
        next: (res) => {
          this.showToast('success', 'Success', 'Add compartment successfully!');
          this.compartmentForm.reset();
          this.isSubmitted = false;
          this.errorMessages = [];
        },
        error: (err) => {
          this.errorMessages = err.error.errors;
          this.showToast('danger', 'Failed', 'Add compartment failed!');
        },
      });
    }
  }

  loadAllCarriage() {
    this.carriageService.getAllCarriageNoPaging().subscribe(res => {
      this.options = res;
      this.filteredOptions$ = of(this.options);
    });
  }

  private filter(value: string): Carriage[] {
    const filterValue = value.toLowerCase();
    return this.options.filter(option => option.name.toLowerCase().includes(filterValue));
  }

  getFilteredOptions(value: string): Observable<Carriage[]> {
    return of(value).pipe(
      map(filterString => this.filter(filterString)),
    );
  }

  onChange() {
    this.isCarriageSelected = false;
    this.errorMessages = [];
    this.filteredOptions$ = this.getFilteredOptions(this.input.nativeElement.value);
  }

  onBlur() {
    const inputValue = this.input.nativeElement.value;
    const matchingCarriage = this.options.find(carriage => carriage.name === inputValue);

    if (!matchingCarriage) {
      this.compartmentForm.patchValue({ carriageId: null });
      this.isCarriageSelected = false;
    }
  }

  onSelectionChange(carriage: Carriage) {
    this.isCarriageSelected = true;
    this.compartmentForm.patchValue({ carriageId: carriage.id });
    this.input.nativeElement.value = carriage.name;
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
