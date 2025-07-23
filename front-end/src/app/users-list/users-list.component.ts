import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatCardModule } from '@angular/material/card';
import { UserService } from '../services/user.service';
import { EventService } from '../services/event.service';
import { User } from '../models/user.model';
import { trigger, state, transition, style, animate } from '@angular/animations';
import { ChangeDetectorRef } from '@angular/core';
import { Event } from '../models/event.model';
import { RouterModule } from '@angular/router';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';

@Component({
  selector: 'app-users-list',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatIconModule,
    MatButtonModule,
    MatInputModule,
    MatCardModule,
    MatSnackBarModule,
    RouterModule
  ],
  providers:[UserService, EventService],
  templateUrl: './users-list.component.html',
  styleUrls: ['./users-list.component.scss'],
  animations: [ trigger('fadeOut', [ transition(':leave', [ animate('300ms ease-out', style({ opacity: 0, transform: 'scale(0.9)' })) ]) ]) ],
})
export class UsersListComponent {
  searchTerm = '';
  searchQuery: string = '';

  users: User[] = [];

  constructor(
    private userService: UserService,
    private eventService: EventService,
    private cdr: ChangeDetectorRef,
    private snackBar: MatSnackBar
  ) {}

  usersPerPage = 5;
  currentPage = 1;

  onSearch() {
    console.log('Search query:', this.searchQuery);
  }

  ngOnInit() {
    this.loadUsers();
  }

  loadUsers() {
    this.userService.getAllUsers().subscribe((res: User[]) => {
      this.users = res.map(u => ({
        ...u,
        name: `${u.first_Name} ${u.middle_Name ?? ''} ${u.last_Name}`.trim(),
        phone: u.phone_Number,
        expanded: false,
        events: []
      }));
    });
  }

  toggleExpand(user: User) {
    user.expanded = !user.expanded;

    if (user.expanded && (!user.events || user.events.length === 0)) {
      this.userService.getUserEvents(user.user_Id).subscribe(events => {
        user.events = events;
      });
    }
  }

  deleteEvent(user: User, eventId: number) {
    this.eventService.removeRsvp(user.user_Id, eventId).subscribe({
      next: () => {
        user.events = user.events?.filter(event => event.id !== eventId) || [];
        
        this.cdr.detectChanges();

        this.snackBar.open('Event RSVP removed successfully.', 'Close', {
          duration: 3000
        });

        console.log('Remaining events:', user.events);
      },
      error: () => {
        this.snackBar.open('Failed to remove RSVP.', 'Close', {
          duration: 3000
        });
      }
    });
  }


  get filteredUsers() {
    return this.users.filter(user =>
      user.name!.toLowerCase().includes(this.searchTerm.toLowerCase())
    );
  }

  trackByUserId(index: number, user: User): number {
    return user.user_Id;
  }

  trackByEventId(index: number, event: Event): number {
    return event.id;
  }

  get totalPages(): number {
    return Math.ceil(this.filteredUsers.length / this.usersPerPage);
  }

  get paginatedUsers(): User[] {
    const startIndex = (this.currentPage - 1) * this.usersPerPage;
    return this.filteredUsers.slice(startIndex, startIndex + this.usersPerPage);
  }

  nextPage() {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
    }
  }

  prevPage() {
    if (this.currentPage > 1) {
      this.currentPage--;
    }
  }
}

