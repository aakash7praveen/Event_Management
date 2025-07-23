import { Component } from '@angular/core';
import { Routes } from '@angular/router';
import { RouterOutlet } from '@angular/router';
import { DashboardComponent } from './dashboard/dashboard.component';
import { CreateEventComponent } from './create-event/create-event.component';
import { UsersListComponent } from "./users-list/users-list.component";
import { AllEventsComponent } from './all-events/all-events.component';
import { AdminLayoutComponent } from './admin-layout/admin-layout.component';
import { UserEventsComponent } from './user-events/user-events.component';
import { AnalyticsComponent } from './analytics/analytics.component';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, DashboardComponent, CreateEventComponent, UsersListComponent, AllEventsComponent, AdminLayoutComponent, UserEventsComponent, AnalyticsComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'Event-Management';
}
