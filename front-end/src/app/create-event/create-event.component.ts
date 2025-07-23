import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field'; 
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatSelectModule } from '@angular/material/select';
import { MatIconModule } from '@angular/material/icon';
import { MatDialogModule, MatDialog } from '@angular/material/dialog';
import { SuccessDialogBoxComponent } from '../shared/success-dialog-box/success-dialog-box.component';
import { ErrorDialogBoxComponent } from '../shared/error-dialog-box/error-dialog-box.component';
import { EventService } from '../services/event.service';
import { Event } from '../models/event.model';
import { Router } from '@angular/router';
import { RouterModule } from '@angular/router';
@Component({
  selector: 'app-create-event',
  imports: [ 
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatSelectModule,
    MatIconModule,
    MatDialogModule,
    RouterModule,
    SuccessDialogBoxComponent,
    ErrorDialogBoxComponent
  ],
  templateUrl: './create-event.component.html',
  styleUrl: './create-event.component.scss'
})
export class CreateEventComponent {
  eventForm: FormGroup;
  

  constructor(private fb: FormBuilder, private eventService: EventService, private dialog: MatDialog, private router: Router){
    this.eventForm = this.fb.group({
      title:['', Validators.required],
      description:[''],
      startTime:[null,Validators.required],
      endTime:[null,Validators.required],
      category:['',Validators.required],
      maxAttendees:[100,[Validators.required,Validators.min(1)]],
      venue:['', Validators.required]
      },{
    validators: this.endAfterStartValidator 
  });

    this.eventForm.get('startTime')?.valueChanges.subscribe(() => {
      this.eventForm.updateValueAndValidity();
    });
    this.eventForm.get('endTime')?.valueChanges.subscribe(() => {
      this.eventForm.updateValueAndValidity();
    });
  }

  increaseAttendees(){
    const current = this.eventForm.value.maxAttendees || 0;
    this.eventForm.patchValue({ maxAttendees: current+1 });
  }

  decreaseAttendees(){
    const current = this.eventForm.value.maxAttendees || 1;
    if (current > 1){
      this.eventForm.patchValue({ maxAttendees: current - 1 });
    }
  }

  endAfterStartValidator(group: FormGroup) {
    const start = new Date(group.get('startTime')?.value);
    const end = new Date(group.get('endTime')?.value);

    return start && end && end <= start ? { endBeforeStart: true } : null;
  }

  saveEvent() {
      if (this.eventForm.invalid) {
        if (this.eventForm.hasError('endBeforeStart')) {
          this.dialog.open(ErrorDialogBoxComponent, {
            data: { message: 'End Date & Time must be <strong>after</strong> Start Date & Time.' }
          });
        } else {
          this.eventForm.markAllAsTouched();
        }
      return;  
    }

    const formValues = this.eventForm.value;
    const currentUser = JSON.parse(localStorage.getItem('currentUser') || '{}');

    const eventPayload = {
      title: formValues.title,
      description: formValues.description,
      startDateTime: formValues.startTime,
      endDateTime: formValues.endTime,
      location: formValues.venue,
      category: formValues.category,
      createdBy: currentUser.userId,
      maxAttendees: formValues.maxAttendees
    };

    this.eventService.createEvent(eventPayload).subscribe({
      next: (res) => {
        if (res.success) {
          const dialogRef = this.dialog.open(SuccessDialogBoxComponent, {
            data: { message: res.message, buttonText: 'Done' }
          });

          dialogRef.afterClosed().subscribe(() => {
            this.router.navigate(['/dashboard']);
          });
        } else {
          this.dialog.open(ErrorDialogBoxComponent, {
            data: { message: 'Something went wrong while creating the event.' }
          });
        }
      },
      error: (err) => {
        this.dialog.open(ErrorDialogBoxComponent, {
          data: { message: err.error?.message || 'Event creation failed.' }
        });
      }
    });
  }

  cancel() {
    this.eventForm.reset();
  }

}
