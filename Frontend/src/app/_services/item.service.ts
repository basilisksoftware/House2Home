import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { JsonPipe } from '@angular/common';
import { AuthService } from './auth.service';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class ItemService {
  baseUrl = environment.apiUrl + 'items/';

  constructor(private http: HttpClient, private auth: AuthService) {}

  toggleCollection(itemId: number) {
    const payload = {
      id: itemId,
    };

    return this.http.post(this.baseUrl + 'toggle-collection', payload, {headers: this.auth.authHeader});
  }
}
