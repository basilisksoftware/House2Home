import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { DataTablesModule } from 'angular-datatables';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AdminComponent } from './admin/admin-auth/admin.component';
import { HomeComponent } from './home/home.component';
import { LoginFormComponent } from './admin/login-form/login-form.component';
import { AdminHomeComponent } from './admin/admin-home/admin-home.component';
import { ErrorPageComponent } from './_layout/error-page/error-page.component';
import { DonateFormReactiveComponent } from './home/donate-form-reactive/donate-form-reactive.component';
import { DonateHeaderComponent } from './home/donate-header/donate-header.component';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { FooterComponent } from './_layout/footer/footer.component';
import { NavbarComponent } from './_layout/navbar/navbar.component';
import { ManageComponent } from './admin/manage/manage.component';
import { ArchiveComponent } from './admin/archive/archive.component';
import { ScheduleComponent } from './admin/schedule/schedule.component';
import { LoadingSpinnerComponent } from './_layout/loading-spinner/loading-spinner.component';
import { ManageTableComponent } from './admin/manage-table/manage-table.component';
import { ManageDonationComponent } from './admin/manage-donation/manage-donation.component';
import { SuccessComponent } from './home/success/success.component';
import { SettingsComponent } from './admin/settings/settings.component';
import { SettingsItemsComponent } from './admin/settings-items/settings-items.component';
import { SettingsUsersComponent } from './admin/settings-users/settings-users.component';
import { StatsBoxComponent } from './admin/stats-box/stats-box.component';


@NgModule({
  declarations: [
    AppComponent,
    AdminComponent,
    HomeComponent,
    LoginFormComponent,
    AdminHomeComponent,
    ErrorPageComponent,
    DonateFormReactiveComponent,
    DonateHeaderComponent,
    FooterComponent,
    NavbarComponent,
    ManageComponent,
    ArchiveComponent,
    ScheduleComponent,
    LoadingSpinnerComponent,
    ManageTableComponent,
    ManageDonationComponent,
    SuccessComponent,
    SettingsComponent,
    SettingsItemsComponent,
    SettingsUsersComponent,
    StatsBoxComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    FontAwesomeModule,
    DataTablesModule
  ],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
