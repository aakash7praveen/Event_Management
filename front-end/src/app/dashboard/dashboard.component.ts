import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatTableModule } from '@angular/material/table';
import { MatDialogModule, MatDialog } from '@angular/material/dialog';
import { EditPopupComponent } from '../shared/edit-popup/edit-popup.component';
import { MatSnackBarModule, MatSnackBar } from '@angular/material/snack-bar';
import { RouterModule } from '@angular/router';
import { WeatherService } from '../services/weather.service';
import { HttpClient } from '@angular/common/http';
import { EventService } from '../services/event.service';
import { Event as EventModel } from '../models/event.model';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, MatButtonModule, MatIconModule, MatCardModule, MatTableModule, MatDialogModule, MatSnackBarModule, RouterModule, EditPopupComponent],
  providers: [EventService],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent {
  weather = {
    city: '',
    temp: 0,
    condition: '',
    icon: ''
  };

  metrics = {
    totalEvents: 0,
    totalUsers: 0,
    upcomingEvents: 0
  };

  currentYear: number = new Date().getFullYear();
  
  todaysEvents: EventModel[] = [];
  
  displayedColumns = ['title', 'startTime', 'endTime', 'location', 'actions'];

  constructor(private dialog: MatDialog, private snackBar: MatSnackBar, private weatherService: WeatherService, private http: HttpClient, private eventService: EventService) {}

    ngOnInit(): void {
      this.loadDashboardMetrics();
      this.loadTodaysEvents();
      this.loadWeather();
    }

    loadDashboardMetrics() {
      this.http.get<any>('https://localhost:7287/api/events/admin/metrics')
        .subscribe({
          next: (data) => this.metrics = {
            totalEvents: data.totalEvents,
            totalUsers: data.totalUsers,
            upcomingEvents: data.upcomingEvents
          },
          error: () => console.error('Failed to load metrics')
        });
    }

    loadTodaysEvents() {
    this.eventService.getTodaysEvents().subscribe({
      next: (events: EventModel[]) => {
        this.todaysEvents = events;
      },
      error: () => console.error('Failed to load today\'s events')
    });
  }

  openEditPopup(event: any) {
    this.eventService.getUsersByEvent(event.id).subscribe(attendees => {
      const dialogRef = this.dialog.open(EditPopupComponent, {
        data: { ...event, attendees: attendees, returnTo: 'dashboard' }, 
        width: '50vw',           
        maxWidth: '50vw',
        height: 'auto',
        panelClass: 'custom-dialog-container',
        disableClose: true,
        autoFocus: false
      });

      dialogRef.afterClosed().subscribe(result => {
        if (result?.updated) {
          this.loadTodaysEvents();
          this.loadDashboardMetrics();
          this.snackBar.open('Event updated successfully!', 'Close', {
            duration: 3000
          });
        }
      });
    });
  }
  
  loadWeather() {
    this.weatherService.getWeather('Kochi').subscribe(data => {
      this.weather = {
        city: data.name,
        temp: Math.round(data.main.temp),
        condition: data.weather[0].main,
        icon: `http://openweathermap.org/img/wn/${data.weather[0].icon}@2x.png`
      };
    });
  }

  logout() {
    localStorage.clear();
    window.location.href = '/login';
  }
}

