import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';

//Service for blocking unauthorized access to component routes
@Injectable({ providedIn: 'root' })
export class RouteGuard implements CanActivate {
  public loggedIn: boolean = false;
    constructor(
      private router: Router,
    ) {
      if (localStorage.getItem('loggedIn')){
        this.loggedIn = true
      }
    }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    if (this.loggedIn) {
            // logged in so return true
            return true;
        }
        // not logged in so redirect to home page
        this.router.navigate(['']);
        return false;
    }
}
