import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class EventService {
  private apiUrl = 'https://localhost:7287/api/events';

  constructor(private http: HttpClient) {}

  getAllEvents(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/list`);
  }

  getTodaysEvents(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/todays-events`);
  }

  createEvent(data: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/create`, data);
  }

  updateEvent(data: any): Observable<any> {
    return this.http.put(`${this.apiUrl}/update`, data);
  }

  cancelEvent(eventId: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${eventId}/cancel`);
  }

  getEventById(id: number): Observable<any> {
    return this.http.get(`${this.apiUrl}/${id}`);
  }

  getUsersByEvent(eventId: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/${eventId}/attendees`);
  }

  rsvpToEvent(data: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/rsvp`, data);
  }

  removeRsvp(userId: number, eventId: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/rsvp/remove?userId=${userId}&eventId=${eventId}`);
  }

  getAdminMetrics(): Observable<any> {
    return this.http.get(`${this.apiUrl}/admin/metrics`);
  }

  hasUserRsvped(userId: number, eventId: number): Observable<boolean> {
    return this.http.get<boolean>(`${this.apiUrl}/has-rsvped?userId=${userId}&eventId=${eventId}`);
  }

  getAnalyticsCategory(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/analytics/category-count`);
  }

  getAnalyticsDaily(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/analytics/daily-count`);
  }

  getAnalyticsVenue(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/analytics/top-venues`);
  }

  getAnalyticsRsvp(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/analytics/rsvp-counts`);
  }
}
