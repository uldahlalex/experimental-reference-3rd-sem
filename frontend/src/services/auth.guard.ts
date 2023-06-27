import {CanActivateFn, Router} from "@angular/router";
import jwtDecode from "jwt-decode";
import {AccountProfileData} from "../models/stateModels";
import {inject} from "@angular/core";

export function authGuard(): CanActivateFn {
  return () => {
    const router: Router = inject(Router);
    let jwt = localStorage.getItem('jwt');
    if (jwt != undefined && (jwtDecode(jwt) as AccountProfileData).endUserId != undefined) {
      return true;
    }
    router.navigate(['login'])
    return false;
  }
}
