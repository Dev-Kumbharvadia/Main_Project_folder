import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RetailerStatsComponent } from './retailer-stats.component';

describe('RetailerStatsComponent', () => {
  let component: RetailerStatsComponent;
  let fixture: ComponentFixture<RetailerStatsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RetailerStatsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RetailerStatsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
