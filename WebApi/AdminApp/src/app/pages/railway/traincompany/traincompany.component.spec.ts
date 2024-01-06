import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TraincompanyComponent } from './traincompany.component';

describe('TraincompanyComponent', () => {
  let component: TraincompanyComponent;
  let fixture: ComponentFixture<TraincompanyComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TraincompanyComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TraincompanyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
