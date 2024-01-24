import {Component, OnInit, ViewChild} from '@angular/core';
import {Observable, of} from 'rxjs';
import {map} from 'rxjs/operators';
import { Ticket } from '../../../../@models/ticket';
import { TicketService } from '../../../passenger-and-ticket/ticket/ticket.service';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import { CancellationRule } from '../../../../@models/cancellationRule';
import { CancellationRuleService } from '../../cancellation-rule/cancellation-rule.service';
import { CancellationService } from '../cancellation.service';
import {NbGlobalPhysicalPosition, NbToastrService} from '@nebular/theme';

@Component({
  selector: 'ngx-add-cancellation',
  templateUrl: './add-cancellation.component.html',
  styleUrls: ['./add-cancellation.component.scss']
})
export class AddCancellationComponent implements OnInit{
  options: Ticket[];
  filteredOptions$: Observable<Ticket[]>;

  cancellationRules: CancellationRule[] = [];
  currentcancellationRule: CancellationRule;

  isValidTicketSelected: boolean = false;

  cancellationForm: FormGroup = this.fb.group({});
  isSubmitted = false;
  errorMessages: string[] = [];

  @ViewChild('autoInput') input: { nativeElement: { value: string; }; };


  constructor(private ticketService: TicketService,
              private cancellationRuleService: CancellationRuleService,
              private cancellationService: CancellationService,
              private toastrService: NbToastrService,
              private fb: FormBuilder) {
  }

  ngOnInit() {
    this.loadTickets();
    this.loadCancellationRules();
    this.initForm();
  }

  private loadTickets() {
    this.ticketService.getAllTicketNoPaging().subscribe(tickets => {
      this.options = tickets;
      this.filteredOptions$ = of(this.options);
    });
  }

  private loadCancellationRules() {
    this.cancellationRuleService.getAllCancellationRuleNoPaging().subscribe({
      next: (res) => {
        this.cancellationRules = res;
      },
    });
  }

  initForm() {
    this.cancellationForm = this.fb.group({
      ticketId: ['', [Validators.required]],
      cancellationRuleId: ['', [Validators.required]],
      reason: [''],
      status: [''],
    });
  }

  private filter(value: string): Ticket[] {
    const filterValue = value.toLowerCase();
    return this.options.filter(option => option.code.toLowerCase().includes(filterValue));
  }

  getFilteredOptions(value: string): Observable<Ticket[]> {
    return of(value).pipe(
      map(filterString => this.filter(filterString)),
    );
  }

  onChange() {
    this.filteredOptions$ = this.getFilteredOptions(this.input.nativeElement.value);
  }

  onSelectionChange(ticket: Ticket) {
    this.cancellationForm.patchValue({
      ticketId: ticket.id,
    });
    this.input.nativeElement.value = ticket.code;
    this.isValidTicketSelected = true; // Train hợp lệ được chọn
  }

  onInputBlur() {
    const inputValue = this.input.nativeElement.value;
    const ticketExists = this.options.some(ticket => ticket.code === inputValue);
    this.isValidTicketSelected = ticketExists;
    if (!ticketExists) {
      this.cancellationForm.patchValue({
        ticketId: '',
      });
    }
  }

  onSubmit() {
    this.isSubmitted = true;
    this.errorMessages = [];
    if (this.cancellationForm.invalid) {
      return;
    }

    this.cancellationService.addCancellation(this.cancellationForm.value).subscribe({
      next: _ => {
        this.showToast('success', 'Success', 'Add cancellation successfully!');
        this.cancellationForm.reset();
        this.isSubmitted = false;
        this.errorMessages = [];
      },
      error: (err) => {
        this.showToast('danger', 'Failed', 'Add cancellation failed!');
        this.errorMessages = err.error.errorMessages;
      },
    });
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
