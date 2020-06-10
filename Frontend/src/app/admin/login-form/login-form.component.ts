import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../_services/auth.service';
import { Router } from '@angular/router';
import * as alertify from 'alertifyjs';

@Component({
  selector: 'app-login-form',
  templateUrl: './login-form.component.html',
  styleUrls: ['./login-form.component.css']
})
export class LoginFormComponent implements OnInit {
  model: any = {};

  constructor(private authService: AuthService, private router: Router) { }

  ngOnInit() {
  }

  login() {
    this.authService.login(this.model).subscribe(next => {
      console.log('success');
      this.router.navigate(['admin/admin-home']);
    }, error => {
      console.log('failed');
      alertify.error('Incorrect username/password!');
    });
  }

  logout() {
    localStorage.removeItem('token');
  }

}
