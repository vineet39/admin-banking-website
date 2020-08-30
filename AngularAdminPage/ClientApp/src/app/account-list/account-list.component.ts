import { Component, OnInit, Input } from '@angular/core';
import { Accounts } from '../../../models/accounts';
import {ActivatedRoute} from '@angular/router';
import { ApiService } from '../../../src/app/services/api.service';
import { Chart } from 'chart.js';
import { Transaction } from '../../../models/transactions';

@Component({
  selector: 'app-account-list',
  templateUrl: './account-list.component.html',
  styleUrls: ['./account-list.component.css']
})
export class AccountListComponent implements OnInit {
  public accounts: Accounts[] = [];
  public savingTransactions: Transaction[] = [];
  public checkingTransactions: Transaction[] = [];
  selectedCustomerId: number;
  selectedEntry: any;
  showGraph = false;
  data: number[] = [];
  constructor(private route: ActivatedRoute, private api: ApiService) { }

  ngOnInit() {
      this.fetchAccountData();
  }
  // Populating accounts data to be shown in the view.
  fetchAccountData() {
    this.route.params.subscribe(params => {
      this.selectedCustomerId = params['id'];
      });
    const source = this.api.get('/accounts/' + this.selectedCustomerId);
    source.subscribe(data => { this.accounts = data; }, error => { console.log(error); });
    source.toPromise().then(x => {
      this.changeDataForView();
      this.fetchSavingTransData();
    });
  }
  // Fetching transactions in savings account for graphing.
  // Pushing the length of data in this.data array for y axis.
  fetchSavingTransData() {
    const source = this.api.get('/transactions/' + this.accounts[0].accountNumber);
    source.subscribe(data => { this.savingTransactions = data; }, error => { console.log(error); });
    source.toPromise().then(x => {
      this.data.push(this.savingTransactions.length);
      if (this.accounts.length > 1) {
        this.fetchCheckingTransData();
      }
    });
  }
  // Fetching transactions in checkings account for graphing.
   // Pushing the length of data in this.data array for y axis.
  fetchCheckingTransData() {
    const source = this.api.get('/transactions/' + this.accounts[1].accountNumber);
    source.subscribe(data => { this.checkingTransactions = data; }, error => { console.log(error); });
    source.toPromise().then(x => {
      this.data.push(this.checkingTransactions.length);
    });
  }
  // Changing the way dates are supposed to appear on the screen.
  changeDataForView() {
      for (let i = 0; i < this.accounts.length; i++) {
        const dateToBeSplit = this.accounts[0].modifyDate.toLocaleString();
        const splittedDate = dateToBeSplit.split('T');
        this.accounts[i].modifyDate = splittedDate[0];
    }
  }
  setBarChart() {
    const ctx = document.getElementById('barChart');
    // Showing and hiding graph every time show/hide graph button is clicked.
    if (this.showGraph === false) {
       ctx.style.display = 'none';
    } else {
      ctx.style.display = 'block';
    }
    const data: number[] = [];
    const labels = [];
      for (const account of this.accounts) {
      labels.push(account.accountNumber);
    }
    const myChart = new Chart(ctx, {
      type: 'bar',
      data: {
        labels: labels,
        datasets: [{
          label: '',
          data: this.data,
          backgroundColor: [
            'rgba(255, 99, 132, 0.2)',
            'rgba(54, 162, 235, 0.2)'
          ],
          borderWidth: 1
        }]
      },
      options: {
        legend: {
          display: false
        },
        responsive: false,
        scales: {
          xAxes: [{
            scaleLabel: {
              display: true,
              labelString: 'Account numbers'
            },
            gridLines: {
              color: 'rgba(0, 0, 0, 0)',
            }
          }],
          yAxes: [{
            ticks: {
              beginAtZero: true,
              callback: function (value) { if (value % 1 === 0) { return value; } }
            },
            scaleLabel: {
              display: true,
              labelString: 'Number of transactions'
            },
            gridLines: {
              color: 'rgba(0, 0, 0, 0)',
            }
          }]
        }
      }
    });
  }
  onSelectionChange(entry) {
    this.selectedEntry = entry;
}

}
