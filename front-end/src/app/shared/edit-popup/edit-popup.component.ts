import { Component, Inject, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatSnackBarModule, MatSnackBar } from '@angular/material/snack-bar';
import { Event } from '../../models/event.model';
import { EventService } from '../../services/event.service';
import { Router } from '@angular/router';
import { trigger, state, transition, style, animate } from '@angular/animations';

@Component({
  selector: 'app-edit-popup',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatIconModule,
    MatButtonModule,
    MatDialogModule,
    MatSnackBarModule
  ],
  providers:[EventService],
  templateUrl: './edit-popup.component.html',
  styleUrls: ['./edit-popup.component.scss'],
  animations: [ trigger('fadeOut', [ transition(':leave', [ animate('300ms ease-out', style({ opacity: 0, transform: 'scale(0.9)' })) ]) ]) ],
})
export class EditPopupComponent {
  event: any;
  attendees: any[];
  paginatedAttendees: any[] = [];

  pageSize = 5;
  currentPage = 1;
  totalPages = 1;
  totalPagesArray: number[] = [];
  originalEndDt!: string;
  dateTimeEdited = false;
  durationEdited = false;

  isEditingDescription = false;
  isEditingDateTime = false;
  isEditingDuration = false;
  isEditingLocation = false;

  constructor(
    public dialogRef: MatDialogRef<EditPopupComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private snackBar: MatSnackBar,
    private eventService: EventService,
    private router: Router
  ) {
    this.event = { ...data };
    this.attendees = Array.isArray(data.attendees) ? [...data.attendees] : [];
    this.originalEndDt = data.endDt;
    this.event.durationMinutes = this.getDurationInMinutes(this.event.startDt, this.event.endDt);
    console.log('Popup received event data:', data);
  }

  ngOnInit() {
    console.log('event.startDt:', this.event.startDt);
    this.attendees = this.event.attendees || [];
    this.event.date = this.formatDate(this.event.startDt);
    this.event.time = this.formatTime(this.event.startDt);
    if (!this.event.durationMinutes || this.event.durationMinutes <= 0) {
      this.event.durationMinutes = this.getDurationInMinutes(this.event.startDt, this.event.endDt);
    }

    this.removeDuplicates();
    this.setupPagination();
  }

  toggleDescriptionEdit() {
    this.isEditingDescription = !this.isEditingDescription;
  }

  toggleEdit(field: string) {
    switch (field) {
      case 'datetime':
        if (this.isEditingDateTime) {
          this.dateTimeEdited = true;
        }
        this.isEditingDateTime = !this.isEditingDateTime;
        break;
      case 'duration':
        this.isEditingDuration = !this.isEditingDuration;
        break;
      case 'location':
        this.isEditingLocation = !this.isEditingLocation;
        break;
    }
  }

  removeDuplicates() {
    const seen = new Set();
    this.attendees = this.attendees.filter(att => {
      const key = att.id || att.email;
      if (seen.has(key)) return false;
      seen.add(key);
      return true;
    });
  }

  formatDate(dateTime: string): string {
    return dateTime.slice(0, 10);
  }

  formatTime(dateTime: string): string {
    return dateTime.slice(11, 16);
  }

  setupPagination() {
    this.totalPages = Math.ceil(this.attendees.length / this.pageSize);
    this.totalPagesArray = Array(this.totalPages).fill(0).map((_, i) => i + 1);
    this.paginateAttendees();
  }

  paginateAttendees() {
    const start = (this.currentPage - 1) * this.pageSize;
    const end = start + this.pageSize;
    this.paginatedAttendees = this.attendees.slice(start, end);
  }

  goToPage(page: number) {
    this.currentPage = page;
    this.paginateAttendees();
  }

  goToPreviousPage() {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.paginateAttendees();
    }
  }

  goToNextPage() {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
      this.paginateAttendees();
    }
  }

  getDurationInMinutes(startDt: string, endDt: string): number {
    const start = new Date(startDt);
    const end = new Date(endDt);

    const diffMs = end.getTime() - start.getTime();
    return diffMs > 0 ? Math.floor(diffMs / 60000) : 0;
  }

  formatDuration(minutes: number): string {
    if (!minutes || minutes <= 0) return '0 minutes';
    return minutes > 99 ? `${minutes} minutes` : `${minutes} min`;
  }

  removeAttendee(userId: number) {
    console.log("Trying to remove userId:", userId, "from event:", this.event?.id);
    
    this.eventService.removeRsvp(userId, this.event.id).subscribe({
      next: () => {
        this.attendees = this.attendees.filter(user => user.user_Id !== userId);
        this.setupPagination();
        this.paginateAttendees(); 
        this.snackBar.open('Attendee removed successfully.', 'Close', { duration: 3000 });
      },
      error: () => {
        this.snackBar.open('Failed to remove attendee.', 'Close', { duration: 3000 });
      }
    });
  }

  saveChanges() {
    const startDateTime = this.combineDateTime(this.event.date, this.event.time);
    if (!startDateTime) {
      console.warn('saveChanges(): invalid startDateTime →', { date: this.event.date, time: this.event.time });
      this.snackBar.open('Invalid date or time', 'Close', { duration: 3000 });
      return;
    }

    let endDateTime: string;
    if (this.dateTimeEdited || this.durationEdited) {
      if (typeof this.event.durationMinutes !== 'number' || this.event.durationMinutes <= 0) {
        console.warn('saveChanges(): invalid durationMinutes →', this.event.durationMinutes);
        this.snackBar.open('Invalid or missing duration.', 'Close', { duration: 3000 });
        return;
      }

      endDateTime = this.calculateEndTime(startDateTime, this.event.durationMinutes);
      console.log(
        `saveChanges(): recalculated → start=${startDateTime}, durationMin=${this.event.durationMinutes}, end=${endDateTime}`
      );
    } else {
      endDateTime = this.originalEndDt;
      console.log('saveChanges(): using original endDt →', endDateTime);
    }

    if (!endDateTime) {
      console.error('saveChanges(): could not determine endDateTime');
      this.snackBar.open('Could not determine end time.', 'Close', { duration: 3000 });
      return;
    }

    const updatedEvent = {
      eventId: this.event.id,
      title: this.event.title,
      description: this.event.description,
      startDt: startDateTime,
      endDt: endDateTime,
      location: this.event.location,
      category: this.event.category || 'onsite',
      maxAttendees: this.event.maxAttendees || 100,
    };

    this.eventService.updateEvent(updatedEvent).subscribe({
      next: () => {
        this.snackBar.open('Event updated successfully!', 'Close', { duration: 3000 });
        this.dialogRef.close({ updated: true });
      },
      error: (err) => {
        console.error('saveChanges(): updateEvent failed', err);
        this.snackBar.open('Failed to update event.', 'Close', { duration: 3000 });
      }
    });
  }

  combineDateTime(date: string, time: string): string {
    if (!date || !time) {
      console.error('Invalid date/time for combineDateTime:', { date, time });
      return ''; 
    }
    return `${date}T${time}:00`;
  }

  private pad(n: number) {
    return n.toString().padStart(2,'0');
  }

  calculateEndTime(startDt: string, durationMinutes: number): string {
    const start = new Date(startDt);
    if (isNaN(start.getTime())) {
      console.error('Invalid startDt in calculateEndTime:', startDt);
      return '';
    }

    if (durationMinutes == null || durationMinutes < 0) {
      console.error('Invalid durationMinutes:', durationMinutes);
      return '';
    }

    const end = new Date(start.getTime());
    end.setMinutes(end.getMinutes() + durationMinutes);

    const year   = end.getFullYear();
    const month  = this.pad(end.getMonth() + 1);
    const day    = this.pad(end.getDate());
    const hour   = this.pad(end.getHours());
    const minute = this.pad(end.getMinutes());
    const second = this.pad(end.getSeconds());

    const localIso = `${year}-${month}-${day}T${hour}:${minute}:${second}`;
    console.log(
      `calculateEndTime(): startLocal=${startDt}, +${durationMinutes}m → endLocal=${localIso}`
    );
    return localIso;
  }

  onDateOrTimeChanged() {
    const newStart = this.combineDateTime(this.event.date, this.event.time);
    if (newStart) {
      this.event.startDt = newStart;
      if (this.event.endDt) {
        this.event.durationMinutes = this.getDurationInMinutes(newStart, this.event.endDt);
      }
    }
  }

  onDurationChanged() {
    const startISO = this.combineDateTime(this.event.date, this.event.time);
    this.event.startDt = startISO;
    this.event.endDt = this.calculateEndTime(startISO, this.event.durationMinutes);
    console.log(`new endDt now = ${this.event.endDt}`);
  }

  trackByAttendeeId(index: number, attendee: any): number {
    return attendee.user_Id;
  }

  onCancel() {
    this.dateTimeEdited = false;
    this.dialogRef.close();
    this.router.navigate([`/${this.data.returnTo}`]);
  }

  onUpdate() {
    this.saveChanges();
    this.dialogRef.close();
    this.router.navigate([`/${this.data.returnTo}`]);
  }

  closePopup(): void {
    this.dialogRef.close();
  }
}
