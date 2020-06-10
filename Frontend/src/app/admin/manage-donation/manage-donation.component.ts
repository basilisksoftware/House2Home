import { Component, OnInit, Input } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DonationService } from 'src/app/_services/donation.service';
import * as alertify from 'alertifyjs';
import {
  faCheckCircle,
  faTimesCircle,
  faBoxOpen,
  faExclamationTriangle,
  faTruck,
  faCheckDouble,
  faArchive,
} from '@fortawesome/free-solid-svg-icons';
import { ItemService } from 'src/app/_services/item.service';
import { AuthService } from 'src/app/_services/auth.service';

@Component({
  selector: 'app-manage-donation',
  templateUrl: './manage-donation.component.html',
  styleUrls: ['./manage-donation.component.css'],
})
export class ManageDonationComponent implements OnInit {
  donationId: number;
  donation: any = ''; // Initialise as empty string because this will be undefined until http request is complete
  collectionDate: Date;
  loading = true;

  // Helpers
  progressButtonText: string;
  numCollections: number;
  isAdmin = this.auth.isAdmin;

  // Icons
  faCheckCircle = faCheckCircle;
  faTimesCircle = faTimesCircle;
  faBoxOpen = faBoxOpen;
  faExclamationTriangle = faExclamationTriangle;
  faTruck = faTruck;
  faCheckDouble = faCheckDouble;
  faArchive = faArchive;

  constructor(
    private route: ActivatedRoute,
    private donationService: DonationService,
    private itemService: ItemService,
    private router: Router,
    private auth: AuthService
  ) {}

  ngOnInit() {
    // alertify.defaults.theme.ok = 'btn btn-primary';
    // alertify.defaults.theme.cancel = 'btn btn-danger';
    // alertify.defaults.theme.input = 'form-control';

    // Get the id of this donation
    this.donationId = this.route.snapshot.params.id;

    // Make a call to the database to get the donation based on the id
    this.donationService.getSingle(this.donationId).subscribe(
      (next) => {
        // Set the donation
        this.donation = next;

        // Count the items for collection
        this.countCollections();
        this.setProgressButtonText();

        // Error if no donation retrieved
        if (this.donation == null) {
          this.router.navigate(['error']);
        }

        // Split picture Urls in each item by ';'
        this.donation.items.forEach((item) => {
          let pUrls = item.pictureUrls;
          pUrls = pUrls.substring(0, pUrls.length - 1);
          const x = pUrls.split(';');
          item.splitUrls = x;
        });

        // Disable loading
        this.loading = false;
      },
      (error) => {
        console.log('error below!');
        this.loading = false;
        this.router.navigate(['error']);
      }
    );
  }

  toggleItemCollection(item: any) {
    this.itemService.toggleCollection(item.id).subscribe(
      (next) => {
        item.collect = !item.collect;
        this.countCollections();
      },
      (error) => {
        console.log(error);
        alertify.error('Oh no, something weird happened! Contact Jazz!');
      }
    );
  }

  setProgressButtonText() {
    switch (this.donation.status) {
      case null:
        this.progressButtonText = 'Accept';
        break;
      case 'Accepted':
        this.progressButtonText = 'Arrange Collection';
        break;
      case 'Awaiting Collection':
        this.progressButtonText = 'Complete';
        break;
      case 'Completed':
        this.progressButtonText = 'Archive';
        break;
      default:
        this.progressButtonText = 'This text should be hidden';
        break;
    }
  }

  countCollections() {
    this.numCollections = this.donation.items.filter(
      (i) => i.collect === true
    ).length;
  }

  nextStage() {
    switch (this.donation.status) {
      case null:
        alertify.confirm(
          'Confirm',
          'Are you sure you want to accept this donation?',
          () => {
            this.setStatus('Accepted');
          },
          () => {
            return;
          }
        );
        break;
      case 'Accepted':
        alertify.confirm(
          'Confirm collection',
          'Are you sure? WARNING: this will send an email to the donor informing them the collection will occur on the date entered ',
          () => {
            this.setStatus('Awaiting Collection');
          },
          () => {
            return;
          }
        );

        break;
      case 'Awaiting Collection':
        alertify
          .confirm(
            'Confirm',
            'Has this donation been collected?',
            () => {
              this.setStatus('Completed');
            },
            () => {
              return;
            }
          )
          .set('labels', { ok: 'Yes', cancel: 'No' });
        break;
      case 'Completed':
        alertify.confirm(
          'Confirm',
          'Archive this donation? WARNING: All personal information will be permanently redacted!',
          () => {
            this.setStatus('Archived');
          },
          () => {
            return;
          }
        );
        break;
      default:
        break;
    }
  }

  setStatus(status: string) {
    // Create DTO
    const model = {
      id: this.donation.id,
      statusToSet: status,
      collectionDate: this.collectionDate,
    };

    // Ensure accepted donations have a collection date in order to progress to
    // 'Awaiting Collection' status and also at least 1 item selected
    if (this.donation.status === 'Accepted') {
      // Check date
      if (
        model.collectionDate === undefined ||
        model.collectionDate === null ||
        model.collectionDate.toString() === ''
      ) {
        alertify.error('Invalid collection date!');
        return;
      }

      // Check at least 1 donation is selected
      let passed = false;
      this.donation.items.forEach((item) => {
        if (item.collect) {
          passed = true;
        }
      });

      // Error if not passed
      if (!passed) {
        alertify.error('At least 1 item must be set to collected!');
        return;
      }
    }
    // End of 'Awaiting Collection' caveat

    // If archiving, remove all personal information (this also updates the status)
    // Execution ends after this block
    if (status === 'Archived') {
      const archiveModel = {
        id: this.donation.id,
        name: this.donation.name,
        email: this.donation.email,
        phone: this.donation.phone,
        address: this.donation.address,
        status: this.donation.status,
      };

      this.donationService.archiveDonation(archiveModel).subscribe(
        (next) => {
          this.router.navigate(['/admin/manage']);
          alertify.success('Donation successfully archived');
        },
        (error) => {
          alertify.error('Unexpected error!');
          console.log('error');
          console.log(error);
          this.router.navigate(['/error']);
        }
      );

      return;
    }
    // End of 'Archive' caveat

    // Call api to update the status
    this.donationService.updateStatus(model).subscribe(
      (next) => {
        console.log('success');
        this.donation.status = status;
        this.setProgressButtonText();
        this.router.navigate(['/admin/manage']);
        alertify.success('Donation successfully set to: ' + status);
      },
      (error) => {
        alertify.error('Unexpected error!');
        console.log('error');
        console.log(error);
        this.router.navigate(['/error']);
      }
    );
  }

  reject() {
    alertify
      .confirm(
        'Confirm',
        'Are you sure you want to reject this donation? WARNING: This action will send an email to the donor!',
        () => {
          this.setStatus('Rejected');
        },
        () => {
          return;
        }
      )
      .set('labels', { ok: 'Yes', cancel: 'No' });
  }
}
