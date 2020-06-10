import { Component, OnInit } from '@angular/core';
import { DonationService } from 'src/app/_services/donation.service';

@Component({
  selector: 'app-manage',
  templateUrl: './manage.component.html',
  styleUrls: ['./manage.component.css'],
})
export class ManageComponent implements OnInit {
  allDonations: any;
  newDonations: any;
  acceptedDonations: any;
  awaitingCollectionDonations: any;
  completeDonations: any;

  loading = false;

  constructor(private donationService: DonationService) {}

  ngOnInit() {
    this.getData();
  }

  getData() {
    this.loading = true;
    this.donationService.getDonations().subscribe(
      (next) => {
        this.allDonations = next;

        this.newDonations = this.allDonations.filter((d) => d.status === null);
        this.acceptedDonations = this.allDonations.filter(
          (d) => d.status === 'Accepted'
        );
        this.awaitingCollectionDonations = this.allDonations.filter(
          (d) => d.status === 'Awaiting Collection'
        );
        this.completeDonations = this.allDonations.filter(
          (d) => d.status === 'Completed'
        );

        this.loading = false;
      },
      (error) => {
        console.log('error occured:');
        console.log(error);
        this.loading = false;
      }
    );
  }
}
