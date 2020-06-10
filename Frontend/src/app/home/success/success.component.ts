import { Component, OnInit } from '@angular/core';
import { faCheckCircle } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-success',
  templateUrl: './success.component.html',
  styleUrls: ['./success.component.css'],
})
export class SuccessComponent implements OnInit {
  
  // Icons
  faCheckCircle = faCheckCircle;

  constructor() {}

  ngOnInit(): void {}
}
