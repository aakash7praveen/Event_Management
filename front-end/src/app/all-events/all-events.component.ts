import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { EditPopupComponent } from '../shared/edit-popup/edit-popup.component';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { EventService } from '../services/event.service';
import { Event } from '../models/event.model';
import { MatSnackBar } from '@angular/material/snack-bar';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-all-events',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatDatepickerModule,
    MatIconModule,
    MatButtonModule,
    RouterModule
  ],
  providers: [EventService],
  templateUrl: './all-events.component.html',
  styleUrls: ['./all-events.component.scss']
})
export class AllEventsComponent {
  searchText = '';
  selectedVenue = '';
  selectedDate: Date | null = null;

  allEvents: Event[] = [];
  venues: string[] = [];

  currentPage: number = 1;
  pageSize: number = 9;

  constructor(private dialog: MatDialog, private eventService: EventService, private snackBar: MatSnackBar) {}

  ngOnInit(): void {
    this.eventService.getAllEvents().subscribe({
      next: (events) => {
        this.allEvents = events;
        this.extractVenues(events);
      },
      error: () => console.error('Failed to fetch events')
    });
  }

  get filteredEvents() {
    return this.allEvents.filter(event => {
      const matchesTitle = this.searchText === '' || event.title.toLowerCase().includes(this.searchText.toLowerCase());
      const matchesVenue = this.selectedVenue === '' || event.location === this.selectedVenue;
      const matchesDate = !this.selectedDate || this.formatDate(new Date(event.startDt)) === this.formatDate(this.selectedDate);
      return matchesTitle && matchesVenue && matchesDate;
    });
  }

  extractVenues(events: Event[]): void {
    const unique = new Set(events.map(e => e.location));
    this.venues = Array.from(unique);
  }

  formatDate(date: Date): string {
    const d = new Date(date);
    return `${d.getMonth() + 1}/${d.getDate()}/${d.getFullYear()}`;
  }

  get totalPages(): number {
    return Math.ceil(this.filteredEvents.length / this.pageSize);
  }

  get pagedEvents(): any[] {
    const start = (this.currentPage - 1) * this.pageSize;
    return this.filteredEvents.slice(start, start + this.pageSize);
  }

  nextPage() {
    if (this.currentPage < this.totalPages) this.currentPage++;
  }

  prevPage() {
    if (this.currentPage > 1) this.currentPage--;
  }

  openEditPopup(event: any) {
    this.eventService.getUsersByEvent(event.id).subscribe(attendees => {
      const dialogRef = this.dialog.open(EditPopupComponent, {
        data: { ...event, attendees: attendees, returnTo: 'manage-event' },
        width: '50vw',           
        maxWidth: '50vw',
        height: 'auto',
        panelClass: 'custom-dialog-container',
        disableClose: true,
        autoFocus: false
      });

      dialogRef.afterClosed().subscribe(result => {
        if (result?.updated) {
          const index = this.allEvents.findIndex(e => e.id === event.id);
          if (index !== -1) {
            this.eventService.getAllEvents().subscribe(events => {
              this.allEvents = events;
            });
          }

          this.snackBar.open('Event updated successfully!', 'Close', {
            duration: 3000
          });
        }
      });
    });
  }


  confirmCancel(event: Event): void {
    const confirm = window.confirm(`Are you sure you want to cancel "${event.title}"?`);
    if (!confirm) return;

    this.eventService.cancelEvent(event.id).subscribe({
      next: () => {
        this.allEvents = this.allEvents.filter(e => e.id !== event.id);
        this.snackBar.open('Event cancelled successfully.', 'Close', { duration: 3000 });
      },
      error: () => {
        this.snackBar.open('Failed to cancel event.', 'Close', { duration: 3000 });
      }
    });
  }
}
