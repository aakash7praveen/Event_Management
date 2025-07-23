import { Routes } from '@angular/router';
import { LoginComponent } from './auth/login/login.component';
import { SignupComponent } from './auth/signup/signup.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { CreateEventComponent } from './create-event/create-event.component';
import { AllEventsComponent } from './all-events/all-events.component';
import { UsersListComponent } from './users-list/users-list.component';
import { AdminLayoutComponent } from './admin-layout/admin-layout.component';
import { UserEventsComponent } from './user-events/user-events.component';
import { AnalyticsComponent } from './analytics/analytics.component';

export const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },

  { path: 'login', component: LoginComponent },
  { path: 'signup', component: SignupComponent },
  { path: 'user-events', component: UserEventsComponent },

  {
    path: '',
    component: AdminLayoutComponent,
    children: [
      { path: 'dashboard', component: DashboardComponent },
      { path: 'create-event', component: CreateEventComponent },
      { path: 'manage-event', component: AllEventsComponent },
      { path: 'users', component: UsersListComponent },
      { path: 'analytics', component: AnalyticsComponent }
    ]
  },

  { path: '**', redirectTo: 'login' }
];
