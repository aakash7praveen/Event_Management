import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ErrorDialogBoxComponent } from './error-dialog-box.component';

describe('ErrorDialogBoxComponent', () => {
  let component: ErrorDialogBoxComponent;
  let fixture: ComponentFixture<ErrorDialogBoxComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ErrorDialogBoxComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ErrorDialogBoxComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
