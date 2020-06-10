import { Component, OnInit } from '@angular/core';
import { Statistics } from 'src/app/_models/statistics.model';
import { StatsService } from 'src/app/_services/stats.service';
import { AuthService } from 'src/app/_services/auth.service';

@Component({
  selector: 'app-admin-home',
  templateUrl: './admin-home.component.html',
  styleUrls: ['./admin-home.component.css'],
})
export class AdminHomeComponent implements OnInit {
  stats = new Statistics();
  loading = true;

  constructor(private statsService: StatsService, private auth: AuthService) {}

  ngOnInit() {
    if (this.auth.isAdmin) {
      this.statsService.getStatistics().subscribe(
        (next) => {
          this.stats = next;
          this.loading = false;
          console.log(this.stats);
        },
        (error) => {
          console.log(error);
          this.loading = false;
        }
      );
    }
  }
}
