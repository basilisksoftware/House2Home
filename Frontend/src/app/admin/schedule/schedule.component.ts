import { Component, OnInit } from '@angular/core';
import { DonationService } from 'src/app/_services/donation.service';
import { Router } from '@angular/router';
import * as alertify from 'alertifyjs';

@Component({
  selector: 'app-schedule',
  templateUrl: './schedule.component.html',
  styleUrls: ['./schedule.component.css'],
})
export class ScheduleComponent implements OnInit {

  loading = false;
  awaitingDonations: any;

  constructor(private donationService: DonationService, private router: Router) {}

  ngOnInit(): void {
    this.loading = true;
    this.donationService.getSchedule().subscribe(
      (next) => {
        console.log(next);
        this.awaitingDonations = next;
        this.loading = false;
      },
      (error) => {
        alertify.error('Unexpected error!');
        this.router.navigate(['error']);
        console.log(error);
        this.loading = false;
      }
    );
  }
}
