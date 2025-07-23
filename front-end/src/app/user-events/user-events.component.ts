import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { RouterModule,Router } from '@angular/router';
import { SuccessDialogBoxComponent } from '../shared/success-dialog-box/success-dialog-box.component';
import { ErrorDialogBoxComponent } from '../shared/error-dialog-box/error-dialog-box.component';
import { FormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { UserService } from '../services/user.service';      
import { Event } from '../models/event.model';
import { forkJoin, map, Observable } from 'rxjs';
import { EventService } from '../services/event.service';
import { MatMenuModule } from '@angular/material/menu';
import { trigger, transition, style, animate } from '@angular/animations';

@Component({
  selector: 'app-user-events',
  standalone: true,
  imports: [
    CommonModule,
    MatDialogModule,
    MatButtonModule,
    MatFormFieldModule,
    MatCardModule,
    MatIconModule,
    MatMenuModule,
    RouterModule,
    FormsModule,
    MatInputModule,
    MatSelectModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatButtonToggleModule,
    SuccessDialogBoxComponent,
    ErrorDialogBoxComponent,
  ],
  providers: [UserService, EventService],
  templateUrl: './user-events.component.html',
  styleUrls: ['./user-events.component.scss']
})
export class UserEventsComponent {
  activeTab: string = 'upcoming';
  filters = {
    title: '',
    venue: '',
    date: ''
  };

  events: Event[] = [];
  filteredEvents: Event[] = [];
  upcomingEvents: Event[] = [];
  acceptedEvents: Event[] = [];
  pastEvents: Event[] = [];


  currentUser: {
    id: number;
    firstName: string;
    profilePicture?: string;
  } = {
    id: 0,
    firstName: '',
    profilePicture: ''
  };
  profilePictureUrl: string = '';

  venues: string[] = [];

  currentYear: number = new Date().getFullYear();

  showWelcome = true;

  constructor(private dialog: MatDialog, private userService: UserService, private eventService: EventService, private router: Router) {}

  ngOnInit(): void {
    const user = JSON.parse(localStorage.getItem('currentUser') || '{}');
    this.currentUser = {
      id: user.userId,
      firstName: user.firstName,
      profilePicture: user.profilePicture || ''
    };

    this.profilePictureUrl = this.currentUser.profilePicture
      ? `https://localhost:7287${this.currentUser.profilePicture}`
      : 'assets/images/default-image.png';
   
    setTimeout(() => {
      this.showWelcome = false;
    }, 5000);

    this.loadEventsByTab(this.activeTab);
  }

  loadEventsByTab(tab: string) {
    if (!tab || typeof tab !== 'string') {
      console.error('Invalid tab value:', tab);
      return;
    }

    console.log('Tab switched to:', tab);
    this.activeTab = tab.trim();  
    this.filters = {
      title: '',
      venue: '',
      date: ''
    };

    let fetchFn: Observable<Event[]> | undefined;

    switch (this.activeTab) {
      case 'upcoming':
        fetchFn = this.userService.getUpcomingEvents(this.currentUser.id);
        break;
      case 'accepted':
        fetchFn = this.userService.getAcceptedEvents(this.currentUser.id);
        break;
      case 'past':
        fetchFn = this.userService.getPastEvents(this.currentUser.id);
        break;
      default:
        console.warn('Unknown tab type:', tab);
        return; 
    }

    if (!fetchFn) {
      console.error('No fetch function available for tab:', tab);
      return;
    }

    fetchFn.subscribe({
      next: (events) => {
        console.log(`${tab} events loaded:`, events);

        switch (this.activeTab) {
          case 'upcoming':
            this.upcomingEvents = [...events];
            break;
          case 'accepted':
            this.acceptedEvents = [...events];
            break;
          case 'past':
            this.pastEvents = [...events];
            break;
        }

        this.updateVenueOptions(events);
        this.applyFilters();
      },
      error: (err) => {
        console.error(`${tab} events load failed:`, err);
      }
    });
  }

  updateVenueOptions(events: Event[]): void {
    const venueSet = new Set<string>();
    events.forEach(event => {
      if (event.location) {
        venueSet.add(event.location);
      }
    });
    this.venues = Array.from(venueSet);
  }

  setFilteredEvents(events: Event[]): void {
    this.events = events;
    this.applyFilters();
  }

  logout(): void {
    localStorage.removeItem('currentUser');
    this.router.navigate(['/login']);
  }

  applyFilters(): void {
    let baseEvents: Event[] = [];

    switch (this.activeTab) {
      case 'upcoming':
        baseEvents = this.upcomingEvents || [];
        break;
      case 'accepted':
        baseEvents = this.acceptedEvents || [];
        break;
      case 'past':
        baseEvents = this.pastEvents || [];
        break;
      default:
        baseEvents = [];
        break;
    }

    console.log('Active Tab:', this.activeTab);
    console.log('Base Events:', baseEvents);

    this.filteredEvents = baseEvents.filter(event => {
      const titleMatch = !this.filters.title || event.title?.toLowerCase().includes(this.filters.title.toLowerCase());
      const venueMatch = !this.filters.venue || event.location === this.filters.venue;
      const dateMatch = !this.filters.date || new Date(event.startDt).toDateString() === new Date(this.filters.date).toDateString();
      return titleMatch && venueMatch && dateMatch;
    });
  }

  onFilterChange(): void {
    this.applyFilters();
  }

  rsvpAndAcceptEvent(event: Event): void {
    const eventDate = new Date(event.startDt);
    const now = new Date();

    if (eventDate < now) {
      this.dialog.open(ErrorDialogBoxComponent, {
        data: {
          message: 'This event has already ended. You can no longer RSVP.',
          buttonText: 'Okay'
        }
      });
      return;
    }

    
    this.eventService.hasUserRsvped(this.currentUser.id, event.id).subscribe(hasRsvped => {
      if (hasRsvped) {
        this.dialog.open(ErrorDialogBoxComponent, {
          data: {
            message: 'You have already RSVP’d to this event.',
            buttonText: 'Okay'
          }
        });
        return;
      }

      this.eventService.rsvpToEvent({
        userId: this.currentUser.id,
        eventId: event.id,
        status: 'confirmed'
      }).subscribe({
        next: (response) => {
          const eventDateTime = new Date(event.startDt).toLocaleString('en-US', {
            weekday: 'long',
            year: 'numeric',
            month: 'short',
            day: 'numeric',
            hour: '2-digit',
            minute: '2-digit'
          });
          const successMessage = `Thank you for confirming your RSVP! We’re excited to see you at <strong>${event.title}</strong> on <strong>${eventDateTime}</strong> at <strong>${event.location}</strong>.`;
          console.log("RSVP Success Response", response);
          this.dialog.open(SuccessDialogBoxComponent, {
            data: {
              message: successMessage,
              buttonText: 'Done'
            }
          });
          this.loadEventsByTab('accepted');
        },
        error: (error) => {
          console.log("RSVP Failure Response", error);
          this.dialog.open(ErrorDialogBoxComponent, {
            data: {
              message: 'RSVP failed. Try again later.',
              buttonText: 'Okay'
            }
          });
        }
      });
    });
  }
}
