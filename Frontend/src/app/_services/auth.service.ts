import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { JwtHelperService } from '@auth0/angular-jwt';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  baseUrl = environment.apiUrl + 'auth/';
  jwtHelper = new JwtHelperService();

  constructor(private http: HttpClient) {}

  login(model: any) {
    return this.http.post(this.baseUrl + 'login', model).pipe(
      map((res: any) => {
        if (res.token) {
          localStorage.setItem('token', res.token);
        }
      })
    );
  }

  loggedIn() {
    const token = localStorage.getItem('token');
    return !this.jwtHelper.isTokenExpired(token);
  }

  get authHeader(): HttpHeaders {
    const reqHeaders = new HttpHeaders({
      Authorization: 'Bearer ' + localStorage.getItem('token'),
    });

    return reqHeaders;
  }

  get isAdmin(): boolean {
    const decodedToken = this.jwtHelper.decodeToken(
      localStorage.getItem('token')
    );

    if (decodedToken.role === 'admin') {
      return true;
    } else {
      return false;
    }
  }

  get username(): string {
    const decodedToken = this.jwtHelper.decodeToken(
      localStorage.getItem('token')
    );

    return decodedToken.unique_name;
  }
}
