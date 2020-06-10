import { Component, OnInit } from '@angular/core';
import {
  faCogs,
  faUser,
  faSitemap,
  faChair,
} from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.css'],
})
export class SettingsComponent implements OnInit {
  // Icons
  faCogs = faCogs;
  faUser = faUser;
  faSitemap = faSitemap;
  faChair = faChair;

  selectedSetting = 'items';

  constructor() {}

  ngOnInit(): void {}
}
