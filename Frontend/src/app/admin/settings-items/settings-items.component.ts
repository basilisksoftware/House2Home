import { Component, OnInit } from '@angular/core';
import {
  faTrashAlt,
  faPlusCircle,
  faFireAlt,
} from '@fortawesome/free-solid-svg-icons';
import { HttpClient } from '@angular/common/http';
import { CategoryService } from 'src/app/_services/category.service';
import * as alertify from 'alertifyjs';

@Component({
  selector: 'app-settings-items',
  templateUrl: './settings-items.component.html',
  styleUrls: ['./settings-items.component.css'],
})
export class SettingsItemsComponent implements OnInit {
  categories: any = [];
  categoryName: string;
  verifyFireLabels: any;
  loading = false;

  // Icons
  faTrashAlt = faTrashAlt;
  faPlusCircle = faPlusCircle;
  faFireAlt = faFireAlt;

  constructor(private http: HttpClient, private catService: CategoryService) {}

  ngOnInit(): void {
    this.loading = true;

    // Get categories
    this.catService.getCategories().subscribe(
      (next) => {
        this.categories = next;
        this.loading = false;
      },
      (error) => {
        console.log(error);
        alertify.error('Unexpected error!');
        this.loading = false;
      }
    );
  }

  addCategory() {
    const model: any = {
      categoryName: this.categoryName,
      verifyFireLabels: this.verifyFireLabels,
    };

    // Verify Fire Labels should be false if never checked (undefined)
    if (model.verifyFireLabels === undefined) {
      model.verifyFireLabels = false;
    }

    // Check name entered
    if (this.categoryName === undefined || this.categoryName === null ) {
      alertify.error('Enter the category name first!');
      return;
    }

    this.catService.addCategory(model).subscribe(
      (next) => {
        this.categories.push(next);
        alertify.success(model.categoryName + ' added');
      },
      (error) => {
        console.log(error);
        alertify.error('Unexpected error!');
      }
    );
  }

  removeCategory(id: number, index: number) {
    this.catService.removeCategory(id).subscribe(
      (next) => {
        this.categories.splice(index, 1);
        alertify.success('Deleted');
      },
      (error) => {
        console.log(error);
        alertify.error('Unexpected error!');
      }
    );
  }
}
