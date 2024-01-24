import {Component, OnInit, ViewChild} from '@angular/core';
import { Ticket } from '../../../../@models/ticket';
import {Observable, of} from 'rxjs';
import { CancellationRule } from '../../../../@models/cancellationRule';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import { TicketService } from '../../../passenger-and-ticket/ticket/ticket.service';
import { CancellationRuleService } from '../../cancellation-rule/cancellation-rule.service';
import { CancellationService } from '../cancellation.service';
import {NbGlobalPhysicalPosition, NbToastrService} from '@nebular/theme';
import {map} from 'rxjs/operators';
import {ActivatedRoute, Router} from '@angular/router';

@Component({
  selector: 'ngx-edit-cancellation',
  templateUrl: './edit-cancellation.component.html',
  styleUrls: ['./edit-cancellation.component.scss']
})
export class EditCancellationComponent implements OnInit {
  options: Ticket[];
  filteredOptions$: Observable<Ticket[]>;

  cancellationRules: CancellationRule[] = [];
  currentcancellationRule: CancellationRule;

  isValidTicketSelected: boolean = false;

  updateForm: FormGroup = this.fb.group({});
  isSubmitted = false;
  errorMessages: string[] = [];

  isLoading = false;

  @ViewChild('autoInput') input: { nativeElement: { value: string; }; };


  constructor(private ticketService: TicketService,
              private cancellationService: CancellationService,
              private cancellationRuleService: CancellationRuleService,
              private toastrService: NbToastrService,
              private activatedRoute: ActivatedRoute,
              private router: Router,
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
    this.updateForm = this.fb.group({
      id: ['', Validators.required],
      ticketId: ['', [Validators.required]],
      cancellationRuleId: ['', [Validators.required]],
      status: [''],
      reason: [''],
    });

    const id = this.activatedRoute.snapshot.params.id;
    this.isLoading = true;
    this.cancellationService.getCancellationById(id).subscribe({
      next: (res) => {
        this.updateForm.patchValue(res);

        this.ticketService.getTicketById(res.ticketId).subscribe({
          next: (ticket) => {
            this.input.nativeElement.value = ticket.code;
            this.isValidTicketSelected = true;
            this.isLoading = false;
          },
        });
      },
      error: (err) => {
        this.showToast('danger', 'Failed', 'Cancellation doest not exist!');
        this.router.navigateByUrl('/managements/payment-and-cancellation/cancellation');
      },
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
    this.updateForm.patchValue({
      ticketId: ticket.id,
    });
    this.input.nativeElement.value = ticket.code;
    this.isValidTicketSelected = true;
  }

  onInputBlur() {
    const inputValue = this.input.nativeElement.value;
    const ticketExists = this.options.some(ticket => ticket.code === inputValue);
    this.isValidTicketSelected = ticketExists;
    if (!ticketExists) {
      this.updateForm.patchValue({
        ticketId: '',
      });
    }
  }

  onSubmit() {
    this.isSubmitted = true;
    this.errorMessages = [];
    if (this.updateForm.invalid) {
      return;
    }

    this.cancellationService.updateCancellation(this.updateForm.value).subscribe({
      next: _ => {
        this.showToast('success', 'Success', 'Update cancellation successfully!');
        this.updateForm.reset();
        this.isSubmitted = false;
        this.errorMessages = [];
      },
      error: (err) => {
        this.showToast('danger', 'Failed', 'Update cancellation failed!');
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
