import { Component, OnInit } from '@angular/core';
import { Customer } from '../../../models/customers';
import { ActivatedRoute, Router } from '@angular/router';
import { NgForm } from '@angular/forms';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { ApiService } from '../services/api.service';

@Component({
  selector: 'app-edit-profile',
  templateUrl: './edit-profile.component.html',
  styleUrls: ['./edit-profile.component.css']
})
export class EditProfileComponent implements OnInit {
  selectedCustomerId: number;
  customerToBeEdited: Customer;
  editProfileForm: FormGroup;
  constructor(private router: Router, private route: ActivatedRoute, private _fb: FormBuilder, private api: ApiService) {
    this.route.params.subscribe(params => {
      this.selectedCustomerId = params['id'];
    });
    this.editProfileForm = _fb.group({
      customername: ['', [Validators.required, Validators.maxLength(50)]],
      phone: ['', [Validators.required, Validators.pattern('^[(]61[)][\\s][-][\\s][1-9]\\d{7}$')]],
      tfn: ['', [Validators.pattern('[0-9]\\d{10}')]],
      address: ['', [Validators.maxLength(50)]],
      city: ['', [Validators.pattern('^[A-Z][a-z]+$'), Validators.maxLength(40)]],
      state: ['', [Validators.pattern('[A-Z]{3}')]],
      postcode: ['', [Validators.pattern('[1-9]\\d{3}')]]
    });
    this.fetchCustomerData();
   }

  ngOnInit() {
    this.editProfileForm.setValue(this.customerToBeEdited);
    }
  async fetchCustomerData() {
    console.log(this.selectedCustomerId);

    const source = await this.api.get("/customers/" + this.selectedCustomerId);
    source.subscribe(data => { this.customerToBeEdited = data; }, error => { console.log(error); });
    source.toPromise().then(x => {
      if(this.customerToBeEdited)
      this.editProfileForm = this._fb.group({
        customername: [this.customerToBeEdited.customerName, [Validators.required, Validators.maxLength(50)]],
        phone: [this.customerToBeEdited.phone, [Validators.required, Validators.pattern('^[(]61[)][\\s][-][\\s][1-9]\\d{7}$')]],
        tfn: [this.customerToBeEdited.tfn, [Validators.pattern('[0-9]\\d{10}')]],
        address: [this.customerToBeEdited.address, [Validators.maxLength(50)]],
        city: [this.customerToBeEdited.city, [Validators.pattern('^[A-Z][a-z]+$'), Validators.maxLength(40)]],
        state: [this.customerToBeEdited.state, [Validators.pattern('[A-Z]{3}')]],
        postcode: [this.customerToBeEdited.postCode, [Validators.pattern('[1-9]\\d{3}')]]
      });
      this.editProfileForm.setValue(this.customerToBeEdited);
    })
  }

  editProfile() {
    if (!this.editProfileForm.valid) {
      return;
    }
    this.customerToBeEdited = this.editProfileForm.value;
    this.customerToBeEdited.customerID = this.selectedCustomerId;
    this.api.post("/updatecustomer", this.customerToBeEdited).subscribe(error => console.log(error));
      this.router.navigate(['\customer-list']);
  }


}
