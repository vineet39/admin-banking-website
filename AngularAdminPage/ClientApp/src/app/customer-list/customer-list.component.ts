import { Component, OnInit } from '@angular/core';
import { Customer } from '../../../models/customers';
import * as $ from 'jquery';
import { ActivatedRoute, Router } from '@angular/router';
import { ApiService } from '../services/api.service';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Component({
  selector: 'app-customer-list',
  templateUrl: './customer-list.component.html',
  styleUrls: ['./customer-list.component.css']
})
export class CustomerListComponent implements OnInit {
  public customers: Customer[] = [];
  selectedEntry: number;
  constructor(private api: ApiService, private router: Router) { }

  ngOnInit() {
    this.fetchCustomerData();
    // Fetching customer data when a component navigates to this component.
    this.router.events.subscribe(val => {
      this.fetchCustomerData();
  });
  }
  // Populating customers data to be shown in the view.
  fetchCustomerData() {
    this.api.get('/customers')
      .subscribe(data => { this.customers = data; }, error => { console.log(error); });
  }
  // Setting the id of the selected customer id each time admin selects one.
  onSelectionChange(entry) {
    this.selectedEntry = entry;
  }
  // Deleting selected customer.
  deleteCustomerData() {
    const source = this.api.post('/deletecustomer', this.selectedEntry);
    source.toPromise().then(x => this.fetchCustomerData());
  }
  // Toggling the lock set on a customer's user account.
  toggleLock(id: string) {
    this.api.post('/togglelock', id).subscribe(error => {console.log(error); });
  }

}
