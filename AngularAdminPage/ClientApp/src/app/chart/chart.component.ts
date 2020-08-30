import { Component, OnInit } from '@angular/core';
import { Chart } from 'chart.js';
import { Transaction } from '../../../models/transactions';
import {ActivatedRoute} from '@angular/router';
import { ApiService } from '../../../src/app/services/api.service';

@Component({
  selector: 'app-chart',
  templateUrl: './chart.component.html',
  styleUrls: ['./chart.component.css']
})
export class ChartComponent implements OnInit {
  public transactions: Transaction[] = [];
  selectedAccountId: number;
  selectedCustomerId: number;
  startDate: any;
  endDate: any;
  constructor(private route: ActivatedRoute, private api: ApiService) { }

  ngOnInit() {
    // Getting the filters set in previous page.
    this.route.params.subscribe(params => {
      this.selectedCustomerId = params['customerid'];
      this.selectedAccountId = params['accountid'];
      this.startDate = params['startdate'];
      this.endDate = params['enddate'];
     });
     this.fetchTransactionData();
  }
  // Getting filtered transaction data based on filters.
  fetchTransactionData() {
    let source;
    if (this.startDate !== '' && this.endDate !== '') {
      source = this.api
      .get('/transactions/' + this.selectedAccountId + ':' + this.startDate + ':' + this.endDate);
    } else {
      source = this.api.get('/transactions/' + this.selectedAccountId);
    }
    source.subscribe(data => { this.transactions = data; }, error => { console.log(error); });
    // Setting up data for the charts after data is fetched.
    source.toPromise().then(x => {
      this.changeDataForView();
      this.setPieChart();
      this.setBarChart();
    });
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
        this.transactions[i].transactionType = 'Bill Pay';
      } else {
        this.transactions[i].transactionType = 'Service Charge';
      }
    }
  }
  setBarChart() {
    const ctx = document.getElementById('barChart');
    const labels = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    const monthLabels = ['-01-', '-02-', '-03-', '-04-', '-05-', '-06-', '-07-', '-08-', '-09-', '-10-', '-11-', '-12-'];
    const data = [];
    for (const label of monthLabels) {
      const filteredList = this.transactions.filter(x => x.modifyDate.includes(label));
      data.push(filteredList.length);
    }
    const myChart = new Chart(ctx, {
      type: 'line',
      data: {
        labels: labels,
        datasets: [{
          label: '',
          data: data,
          backgroundColor: [
            'rgba(255, 99, 132, 0.2)',
            'rgba(54, 162, 235, 0.2)',
            'rgba(255, 206, 86, 0.2)',
            'rgba(75, 192, 192, 0.2)',
            'rgba(153, 102, 255, 0.2)'
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
              labelString: 'Month'
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
  setPieChart() {
    const ctx = document.getElementById('pieChart');
    const labels = ['Withdraw', 'Deposit', 'Transfer', 'Bill Pay', 'Service Charge'];
    const data = [];
    let filteredList = this.transactions;
    for (const label of labels) {
      filteredList = this.transactions.filter(x => x.transactionType.includes(label));
      data.push(filteredList.length);
    }
    const myChart = new Chart(ctx, {
      type: 'pie',
      data: {
        labels: labels,
        datasets: [{
          data: data,
          backgroundColor: [
            'rgba(255, 99, 132, 0.2)',
            'rgba(54, 162, 235, 0.2)',
            'rgba(255, 206, 86, 0.2)',
            'rgba(75, 192, 192, 0.2)',
            'rgba(153, 102, 255, 0.2)'
          ]
        }]
      },
      options: {
        legend: {
          display: true
        },
        responsive: false
      }
    });
  }
}
