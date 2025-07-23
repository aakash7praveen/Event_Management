import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class WeatherService {
  apiKey = 'cc66c27939d53ccde68322b75b328020'; 
  baseUrl = 'https://api.openweathermap.org/data/2.5/weather';

  constructor(private http: HttpClient) {}

  getWeather(city: string): Observable<any> {
    return this.http.get(`${this.baseUrl}?q=${city}&units=metric&appid=${this.apiKey}`);
  }
}
