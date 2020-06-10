import { Component, OnInit } from '@angular/core';
import { DonationService } from 'src/app/_services/donation.service';
import { faMinusCircle } from '@fortawesome/free-solid-svg-icons';
import * as alertify from 'alertifyjs';

@Component({
  selector: 'app-archive',
  templateUrl: './archive.component.html',
  styleUrls: ['./archive.component.css'],
})
export class ArchiveComponent implements OnInit {

  loading = false;
  archivedDonations: any;
  rejectedDonations: any = [];
  rejectsLoaded = false;

  // Icons
  faMinusCircle = faMinusCircle;

  constructor(private donationService: DonationService) {}

  ngOnInit(): void {
    this.loading = true;
    this.donationService.getArchive().subscribe(
      (next) => {
        this.archivedDonations = next;
        this.loading = false;
      },
      (error) => {
        alertify.error('Unexpected error!');
        console.log(error);
        this.loading = false;
      }
    );
  }

  getRejects() {
    this.loading = true;
    this.donationService.getRejects().subscribe(
      (next) => {
        this.rejectedDonations = next;
        this.loading = false;
        this.rejectsLoaded = true;
      },
      (error) => {
        alertify.error('Unexpected error!');
        console.log(error);
        this.loading = false;
      }
    );
  }
}
