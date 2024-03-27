import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, map,from, catchError, throwError, BehaviorSubject } from 'rxjs';
import { Register } from '../Interface/Register';


@Injectable({
  providedIn: 'root'
})
export class PrimaryService {


  

  apiUrl ="https://localhost:7204";
  apiUrl1 ="http://localhost:5285"

  constructor(private http: HttpClient) {
   }


  //  login(credentials: any): Observable<any> {
  //   // Assuming you're sending credentials to the server for authentication
  //   return from(
  //     fetch(`${this.apiUrl1}/login`, {
  //       method: 'POST',
  //       credentials: 'include',
  //       headers: {
  //         'Content-Type': 'application/json'
  //       },
  //       body: JSON.stringify(credentials)
  //     })
  //     .then(response => {
  //       if (!response.ok) {
  //         throw new Error('Network response was not ok');
  //       }
  //       return response.json();
  //     })
  //     .catch(error => {
  //       console.error('There was a problem with the fetch operation:', error);
  //       throw error;
  //     })
  //   );
  // }
  


  // login(credentials: any): Observable<any> {
  //   const url = `${this.apiUrl1}/login`;

  //   // Making an HTTP GET request to the login endpoint
  //   return this.http.post<any>(url,credentials).pipe(
  //     catchError((error: HttpErrorResponse) => {
  //       // Handling errors
  //       let errorMessage = '';

  //       if (error.error instanceof ErrorEvent) {
  //         // Client-side error
  //         errorMessage = `An error occurred: ${error.error.message}`;
  //       } else {
  //         // Server-side error
  //         errorMessage = `Server returned code: ${error.status}, error message is: ${error.message}`;
  //       }

  //       console.error(errorMessage);

  //       // Forwarding the error to the component
  //       return throwError(errorMessage);
  //     })
  //   );
  // }




  
  logout(): Observable<any> {
    return this.http.post<any>(`${this.apiUrl1}/logout`, {},{withCredentials:true});
  }

  register(register: Register):Observable<any>{
    return this.http.post<any>(`${this.apiUrl1}/register`,register)
  }


   login(credentials: any): Observable<any> {
    // Assuming you're sending credentials to the server for authentication
    return this.http.post<any>(`${this.apiUrl1}/login`, credentials ,{withCredentials:true});
  }

  //  login(register: any): Observable<register> {
  //   return this.http.get<register>(`${this.apiUrl}/login`).pipe(
  //     map(this.processResponse));
  // }

  // private processResponse(register:register): register{
  //   return {
  //     username: register.username,
  //     password: register.password
  //   };
  // }
    
  isLoggedin:boolean =false;
  getLoggedin():boolean{
    return this.isLoggedin;
  }

  



  //  login(register:any): Observable<any[]> {
  //   return new Observable(observer => {
  //     fetch(`${this.apiUrl}/login`,{
  //       method: 'POST',
  //       headers: {
  //         'Content-Type': 'application/json',
  //       },
  //       body: JSON.stringify(register),
  //     })
  //       .then(response => {
  //         if (!response.ok) {
  //           throw new Error('Network response was not ok');
  //         }
  //         return response.json();
  //       })
  //       .then(data => {
  //         //console.log(data);
  //         observer.next(data);
  //         observer.complete();
  //       })
  //       .catch(error => {
  //         console.error('An error occurred while logging in:', error);
  //         observer.error(error);
  //       });
  //   });
  // }


  // register(register: any): Observable<any[]> {
  //   return new Observable(observer => {
  //     fetch(`${this.apiUrl}/register`, {
  //       method: 'POST',
  //       headers: {
  //         'Content-Type': 'application/json',
  //       },
  //       body: JSON.stringify(register),
  //     })
  //     .then(response => {
  //       if (!response.ok) {
  //         throw new Error('Network response was not ok');
  //       }
  //       return response.json();
  //     })
  //     .then(data => {
  //       observer.next(data);
  //       observer.complete();
  //     })
  //     .catch(error => {
  //       console.error('An error occurred while registering:', error);
  //       observer.error(error);
  //     });
  //   });
  // }

}
