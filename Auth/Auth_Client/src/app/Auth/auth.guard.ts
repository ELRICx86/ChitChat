import { CanActivateFn } from '@angular/router';
import { PrimaryService } from '../services/primary.service';
import { inject} from '@angular/core';
import { Router } from 'express';

export const authGuard: CanActivateFn = (route, state) => {

  /**
   *
   */
  
  const primary : PrimaryService = inject(PrimaryService);
  const router = inject(Router);
  if(primary.getLoggedin()==false){
    return true;
  }
  else{
    router.navigate(['/login']);
    return true;
  }
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
