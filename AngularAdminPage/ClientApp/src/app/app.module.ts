import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouteGuard } from './services/routeguard.service';
import { RouterModule } from '@angular/router';
import { APP_BASE_HREF } from '@angular/common';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CustomerListComponent } from './customer-list/customer-list.component';
import { AccountListComponent } from './account-list/account-list.component';
import { TransactionListComponent } from './transaction-list/transaction-list.component';
import { BillListComponent } from './bill-list/bill-list.component';
import { ChartComponent } from './chart/chart.component';
import { EditProfileComponent } from './edit-profile/edit-profile.component';
import { ApiInterceptor } from './services/APIinterceptor.service';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import {MatDatepickerModule} from '@angular/material';
import {MatNativeDateModule} from '@angular/material';
import {MatFormFieldModule} from '@angular/material';
import {MatInputModule} from '@angular/material';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CustomerListComponent,
    AccountListComponent,
    TransactionListComponent,
    BillListComponent,
    ChartComponent,
    EditProfileComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'customer-list', component: CustomerListComponent, canActivate: [RouteGuard] },
      { path: 'account-list/:id', component: AccountListComponent, canActivate: [RouteGuard] },
      { path: 'transaction-list/:customerid/:accountid', component: TransactionListComponent, canActivate: [RouteGuard] },
      { path: 'bill-list/:customerid/:accountid', component: BillListComponent, canActivate: [RouteGuard] },
      { path: 'chart/:customerid/:accountid/:startdate/:enddate', component: ChartComponent, canActivate: [RouteGuard] },
      { path: 'edit-profile/:id', component: EditProfileComponent, canActivate: [RouteGuard] }
    ]),
    BrowserAnimationsModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatFormFieldModule,
    MatInputModule
  ],
  providers: [{ provide: HTTP_INTERCEPTORS, useClass: ApiInterceptor, multi: true }, [NavMenuComponent]],
  bootstrap: [AppComponent]
})
export class AppModule { }
