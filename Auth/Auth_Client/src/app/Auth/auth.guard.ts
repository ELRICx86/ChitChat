import {  CanActivateFn } from '@angular/router';
import { PrimaryService } from '../services/primary.service';
import { inject} from '@angular/core';
import { Router } from '@angular/router';


export class Permission{
  canActivate():boolean{
    return false;
  }
}


export const authGuard: CanActivateFn = (route, state) => {

  const serv  = inject(PrimaryService);
  const _router = inject(Router);
  let isLoggedIn =  serv.getLoggedin();

  if(isLoggedIn == true){
    return true;
  }
  else{
    alert("you must log in first");
    _router.navigate(['login']);
    return false;
  }
  
  // const primary : PrimaryService = inject(PrimaryService);
  // const router = inject(Router);
  // if(primary.getLoggedin()){
  //   return true;
  // }
  // else{
  //   router.navigate([LoginComponent]);
  //   return true;
  // }
};



// import { CanActivateFn } from '@angular/router';
// import { PrimaryService } from '../services/primary.service';
// import { inject } from '@angular/core';

//  // Define the authGuard function, which implements CanActivateFn interface
// export const authGuard: CanActivateFn = (route, state) => {
//   // Use dependency injection to get an instance of the AuthService
//   const primary =  inject(PrimaryService);

//   // Check if the user is logged in using the AuthService
//   if (primary.getLoggedin()) {
//     return true; // If logged in, allow access to the route
//   } else {
//     return false; // If not logged in, deny access to the route
//   }
// };
