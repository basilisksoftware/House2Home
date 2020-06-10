import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { FormPostedService } from '../_services/form-posted.service';

@Injectable({
  providedIn: 'root',
})
export class SuccessGuard implements CanActivate {
  constructor(private postService: FormPostedService, private router: Router) {}

  canActivate(): boolean {
    if (this.postService.isPosted()) {
      return true;
    } else {
      this.router.navigate(['']);
      return false;
    }
  }
}