import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class FormPostedService {
  posted = false;

  isPosted(): boolean {
    return this.posted ? true : false;
  }

  setPosted(): void {
    this.posted = true;
  }

  setNotPosted(): void {
    this.posted = false;
  }
}
