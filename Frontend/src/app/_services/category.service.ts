import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { AuthService } from './auth.service';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class CategoryService {
  baseUrl = environment.apiUrl + 'category';

  constructor(private http: HttpClient, private auth: AuthService) {}

  getCategories() {
    return this.http.get(this.baseUrl);
  }

  addCategory(model: any) {
    return this.http.post(this.baseUrl, model, { headers: this.auth.authHeader });
  }

  removeCategory(catId: number) {
    return this.http.delete(this.baseUrl + catId, { headers: this.auth.authHeader });
  }
}
