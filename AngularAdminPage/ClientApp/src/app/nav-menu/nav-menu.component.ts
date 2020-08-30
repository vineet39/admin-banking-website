import { Component, OnInit, Injectable, NgModule, Input } from '@angular/core';
import { Router } from '@angular/router';
import { NgForm } from '@angular/forms';
import { BehaviorSubject, Observable } from 'rxjs';
import { RouteGuard } from '../services/routeguard.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit {
  @Input()
  loggedIn: boolean = false;
  isExpanded = false;
  constructor(private router: Router, private guard: RouteGuard, ) {
    if (localStorage.getItem('loggedIn')) {
      this.loggedIn = true
    }
  }

  ngOnInit() {
  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
  // Loggin in admin if successfull and redirecting to customer list page.
  onSubmit(form: NgForm) {
    if (form.value.AdminID === 'admin' && form.value.Password === 'admin') {
      this.guard.loggedIn = true;
      this.loggedIn = true;
      localStorage.setItem("loggedIn", 'true');
      this.router.navigate(['\customer-list']);
    }
  }

  logout() {
    this.loggedIn = false;
    this.guard.loggedIn = false;
    localStorage.clear();
    this.router.navigate(['']);
  }
}
