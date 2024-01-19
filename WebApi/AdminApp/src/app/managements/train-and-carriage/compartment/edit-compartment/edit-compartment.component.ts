import {Component, OnInit, ViewChild} from '@angular/core';
import {Carriage} from '../../../../@models/carriage';
import {Observable, of} from 'rxjs';
import {AbstractControl, FormBuilder, FormGroup, ValidationErrors, ValidatorFn, Validators} from '@angular/forms';
import {CompartmentService} from '../compartment.service';
import {CarriageService} from '../../carriage/carriage.service';
import {NbGlobalPhysicalPosition, NbToastrService} from '@nebular/theme';
import {ActivatedRoute, Router} from '@angular/router';
import {map} from 'rxjs/operators';

@Component({
  selector: 'ngx-edit-compartment',
  templateUrl: './edit-compartment.component.html',
  styleUrls: ['./edit-compartment.component.scss'],
})
export class EditCompartmentComponent implements OnInit {
  options: Carriage[];
  filteredOptions$: Observable<Carriage[]>;
  isCarriageSelected: boolean = false;

  isLoading = false;

  @ViewChild('autoInput') input: { nativeElement: { value: string; }; };

  updateForm: FormGroup = this.fb.group({});
  isSubmitted: boolean = false;
  errorMessages: string[] = [];

  constructor(
    private compartmentService: CompartmentService,
    private carriageService: CarriageService,
    private toastrService: NbToastrService,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private fb: FormBuilder,
  ) {
  }

  ngOnInit(): void {
    this.loadAllCarriage();
    this.initForm();
  }

  initForm() {
    this.updateForm = this.fb.group({
      id: ['', Validators.required],
      name: ['', Validators.required],
      carriageId: ['', Validators.required],
      numberOfSeats: [0, [Validators.required, Validators.min(0), this.numberValidator()]],
      status: [''],
    });

    const id = this.activatedRoute.snapshot.params.id;
    this.isLoading = true;
    this.compartmentService.getCompartmentById(id).subscribe({
      next: (res) => {
        this.updateForm.patchValue(res);

        const matchingCarriage = this.options.find(carriage => carriage.id === res.carriageId);
        if (matchingCarriage) {
          this.input.nativeElement.value = matchingCarriage.name;
          this.isCarriageSelected = true;
        }

        this.isLoading = false;
      },
      error: (err) => {
        this.showToast('danger', 'Failed', 'Compartment does not exist!');
        this.router.navigateByUrl('/managements/train-and-carriage/compartment');
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
    this.errorMessages = [];

    if (!this.isCarriageSelected && this.input.nativeElement.value !== '') {
      this.errorMessages.push('Please select a valid compartment.');
      return;
    }

    if (this.updateForm.valid) {
      this.compartmentService.updateCompartment(this.updateForm.value).subscribe({
        next: (res) => {
          this.showToast('success', 'Success', 'Update compartment successfully!');
          this.isSubmitted = false;
          this.errorMessages = [];
        },
        error: (err) => {
          this.errorMessages = err.error.errors;
          this.showToast('danger', 'Failed', 'Update compartment failed!');
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
    this.toastrService.show(body, title, config);
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
      this.updateForm.patchValue({carriageId: null});
      this.isCarriageSelected = false;
    }
  }

  onSelectionChange(carriage: Carriage) {
    this.isCarriageSelected = true;
    this.updateForm.patchValue({carriageId: carriage.id});
    this.input.nativeElement.value = carriage.name;
  }
}
