import { Component, OnInit } from '@angular/core';
import { Billpay } from '../../../models/billpay';
import { ActivatedRoute } from '@angular/router';
import { ApiService } from '../../../src/app/services/api.service';

@Component({
  selector: 'app-bill-list',
  templateUrl: './bill-list.component.html',
  styleUrls: ['./bill-list.component.css']
})
export class BillListComponent implements OnInit {
  public bills: Billpay[] = [];
  selectedAccountId: number;
  selectedCustomerId: number;
  constructor(private route: ActivatedRoute, private api: ApiService) { }

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.selectedCustomerId = params['customerid'];
      this.selectedAccountId = params['accountid'];
     });
      this.fetchBillData();
  }
  // Populating bills data to be shown in the view.
  fetchBillData() {
    const source = this.api.get('/billpay/' + this.selectedAccountId);
    source.subscribe(data => { this.bills = data; }, error => { console.log(error); });
    source.toPromise().then(x => this.changeDataForView());
  }
  // Changing the way dates are supposed to appear on the screen.
  changeDataForView() {
    for (let i = 0; i < this.bills.length; i++) {
      const dateToBeSplit = this.bills[i].modifyDate.toLocaleString();
      const splittedDate = dateToBeSplit.split('T');
      this.bills[i].modifyDate = splittedDate[0];
      const scheduledDate = this.bills[i].scheduleDate.toLocaleString();
      const scheduledDateModified = scheduledDate.replace('T', ' ');
      this.bills[i].scheduleDate = scheduledDateModified;
      const type = this.bills[i].period.toString();
      // Period is stored as enum on .net side and its ascii value is retrieved in json api call.
      // Onceoff as 'S' which is ascii 83, Monthly as 'M' which is ascii 77, Quaterly as 'Q' which
      // is ascii 81 and Anually as 'Y' which is 89.
      if (type === '83') {
        this.bills[i].period = 'OnceOff';
      } else if (type === '77') {
        this.bills[i].period = 'Monthly';
      } else if (type === '81') {
        this.bills[i].period = 'Quarterly';
      } else if (type === '89') {
        this.bills[i].period = 'Annually';
      } else {}
    }
  }
  // Toggling the lock set on a bill.
  toggleBlock(id: string) {
    this.api.post('/billlock', id).subscribe(error => {console.log(error); });
  }

}
