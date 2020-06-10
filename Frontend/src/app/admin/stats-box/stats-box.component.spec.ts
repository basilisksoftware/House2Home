import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { StatsBoxComponent } from './stats-box.component';

describe('StatsBoxComponent', () => {
  let component: StatsBoxComponent;
  let fixture: ComponentFixture<StatsBoxComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ StatsBoxComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(StatsBoxComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
