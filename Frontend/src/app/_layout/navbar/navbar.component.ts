import { Component, OnInit } from '@angular/core';
import {
  faUser,
  faHome,
  faTasks,
  faHistory,
  faCalendarAlt,
  faCogs,
} from '@fortawesome/free-solid-svg-icons';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/_services/auth.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css'],
})
export class NavbarComponent implements OnInit {

  // Auth
  isAdmin = false;
  username: string;

  // Icons
  faUser = faUser;
  faHome = faHome;
  faTasks = faTasks;
  faHistory = faHistory;
  faCalendarAlt = faCalendarAlt;
  faCogs = faCogs;

  constructor(private router: Router, private authService: AuthService) {}

  ngOnInit() {
    this.isAdmin = this.authService.isAdmin;
    this.username = this.authService.username;
  }

  logout() {
    localStorage.removeItem('token');
    this.router.navigate(['admin']);
  }
}
