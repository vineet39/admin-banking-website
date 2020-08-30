import { Component, OnInit } from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import { Transaction } from '../../../models/transactions';
import {FormControl} from '@angular/forms';
import { ApiService } from '../../../src/app/services/api.service';

@Component({
  selector: 'app-transaction-list',
  templateUrl: './transaction-list.component.html',
  styleUrls: ['./transaction-list.component.css']
})
export class TransactionListComponent implements OnInit {
  public transactions: Transaction[] = [];
  selectedAccountId: number;
  selectedCustomerId: number;
  startDate = new FormControl();
  endDate = new FormControl();
  myFilter = (d: Date): boolean => {
    return d > this.startDate.value;
  }
  constructor(private route: ActivatedRoute, private api: ApiService) { }

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.selectedCustomerId = params['customerid'];
      this.selectedAccountId = params['accountid'];
     });
      this.fetchTransactionData();
  }
  // Populating transactions data to be shown in the view.
  fetchTransactionData() {
    const source = this.api.get('/transactions/' + this.selectedAccountId);
    source.subscribe(data => { this.transactions = data; }, error => { console.log(error); });
    source.toPromise().then(x => this.changeDataForView());
  }
  // Changing the way dates and transaction types are supposed to appear on the screen.
  changeDataForView() {
    for (let i = 0; i < this.transactions.length; i++) {
      const dateToBeSplit = this.transactions[i].modifyDate.toLocaleString();
      const splittedDate = dateToBeSplit.split('T');
      this.transactions[i].modifyDate = splittedDate[0];
      const type = this.transactions[i] .transactionType;
      if (type === 'W') {
        this.transactions[i].transactionType = 'Withdraw';
      } else if (type === 'D') {
        this.transactions[i].transactionType = 'Deposit';
      } else if (type === 'T') {
        this.transactions[i].transactionType = 'Transfer';
      } else if (type === 'B') {
        this.transactions[i].transactionType = 'Bill pay';
      } else {
        this.transactions[i].transactionType = 'Service Charge';
      }
    }
  }
  // Filtering transactions based on user input.
  filterTransactions() {
    const source = this.api
    .get('/transactions/' + this.selectedAccountId + ':' + this.startDate.value.toDateString() + ':' + this.endDate.value.toDateString());
    source.subscribe(data => { this.transactions = data; }, error => { console.log(error); });
    source.toPromise().then(x => this.changeDataForView());
  }
  // Reloading all transactions on filter reset.
  resetFilter( ) {
    this.startDate = new FormControl();
    this.endDate = new FormControl();
    this.fetchTransactionData();
  }

}
