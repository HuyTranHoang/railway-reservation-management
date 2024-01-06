import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TrainstationComponent } from './trainstation.component';

describe('TrainstationComponent', () => {
  let component: TrainstationComponent;
  let fixture: ComponentFixture<TrainstationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TrainstationComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TrainstationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
