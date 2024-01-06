import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TicketpriceComponent } from './ticketprice.component';

describe('TicketpriceComponent', () => {
  let component: TicketpriceComponent;
  let fixture: ComponentFixture<TicketpriceComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TicketpriceComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TicketpriceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
