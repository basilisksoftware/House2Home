import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AuthService } from './auth.service';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class DonationService {
  baseUrl = environment.apiUrl + 'donation/';

  constructor(private http: HttpClient, private auth: AuthService) {}

  addDonation(donation: any) {
    const formData = new FormData();
    formData.append('name', donation.name);
    formData.append('address', donation.address);
    formData.append('phone', donation.phone);
    formData.append('email', donation.email);
    formData.append('smokeFree', donation.smokeFree);
    formData.append('petFree', donation.petFree);

    // Append just the data of the items, without the pictures
    const items: any = [];
    donation.items.forEach((item) => {
      const x: any = {};
      x.description = item.description;
      x.category = item.category;
      x.fireLabels = item.fireLabels;
      items.push(x);
    });

    // Add the new array to the form data
    formData.append('items', JSON.stringify(items));

    // Extract each file from each item and append each to the form data
    donation.items.forEach((item) => {
      // tslint:disable-next-line: prefer-for-of
      for (let index = 0; index < item.pictures.length; index++) {
        const picture = item.pictures[index];
        formData.append('pictures', picture, item.description);
      }
    });

    // Send the request
    return this.http.post(this.baseUrl + 'add', formData);
  }

  getDonations() {
    return this.http.get(this.baseUrl + 'get', {headers: this.auth.authHeader});
  }

  getArchive() {
    return this.http.get(this.baseUrl + 'archive', {headers: this.auth.authHeader});
  }

  getRejects() {
    return this.http.get(this.baseUrl + 'rejects', {headers: this.auth.authHeader});
  }

  getSingle(id: number) {
    return this.http.get(this.baseUrl + `get/${id}`, {headers: this.auth.authHeader});
  }

  updateStatus(model: any) {
    return this.http.post(this.baseUrl + 'update-status', model, {headers: this.auth.authHeader});
  }

  archiveDonation(model: any) {
    return this.http.post(this.baseUrl + 'archive-submission', model, {headers: this.auth.authHeader});
  }

  getSchedule() {
    return this.http.get(this.baseUrl + 'schedule', {headers: this.auth.authHeader});
  }
}
