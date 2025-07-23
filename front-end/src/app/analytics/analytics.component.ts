import { Component } from '@angular/core';
import { EventService } from '../services/event.service';
import { NgxChartsModule } from '@swimlane/ngx-charts';
import { Color, ScaleType } from '@swimlane/ngx-charts';
import { MatIconModule } from '@angular/material/icon';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-analytics',
  standalone: true,
  providers: [EventService],
  templateUrl: './analytics.component.html',
  styleUrls: ['./analytics.component.scss'],
  imports: [NgxChartsModule, MatIconModule, RouterModule],
})
export class AnalyticsComponent {
  view: [number, number] = [600, 350];
  colorScheme: Color = {
    name: 'cool',
    selectable: true,
    group: ScaleType.Ordinal,
    domain: ['#00796b', '#009688', '#26a69a', '#4db6ac', '#80cbc4']
  };

  categoryData: any[] = [];
  dailyData: any[] = [];
  venueData: any[] = [];
  rsvpData: any[] = [];

  loading = false;

  constructor(private eventService: EventService) {}

  ngOnInit() {
    this.loadCharts();
  }

  loadCharts() {
    this.loading = true;

    this.eventService.getAnalyticsCategory().subscribe(data => {
      this.categoryData = (data || []).filter(d => d.category && d.count != null).map(d => ({
        name: d.category,
        value: d.count
      }));
    });

    this.eventService.getAnalyticsDaily().subscribe(data => {
      this.dailyData = (data || []).filter(d => d.date && d.count != null).map(d => ({
        name: new Date(d.date).toLocaleDateString('en-IN'),  
        value: d.count                                        
      }));
    });

    this.eventService.getAnalyticsVenue().subscribe(data => {
      this.venueData = (data || []).filter(d => d.venue && d.count != null).map(d => ({
        name: d.venue,
        value: d.count
      }));
    });

    this.eventService.getAnalyticsRsvp().subscribe(data => {
      this.rsvpData = (data || []).filter(d => d.title && d.rsvpCount != null).map(d => ({
        name: d.title,
        value: d.rsvpCount
      }));
      this.loading = false;
    });
  }

  onSelect(event: any): void{
    console.log('Chart selected:', event);
  }
}
