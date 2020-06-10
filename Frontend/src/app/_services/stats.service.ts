import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Statistics } from '../_models/statistics.model';
import { AuthService } from './auth.service';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class StatsService {
  baseUrl = environment.apiUrl + 'statistics/';

  constructor(private http: HttpClient, private auth: AuthService) {}

  getStatistics() {
    return this.http.get<Statistics>(this.baseUrl, {headers: this.auth.authHeader});
  }
}
