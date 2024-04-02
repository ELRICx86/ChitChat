import { Component } from '@angular/core';
import { PrimaryService } from '../services/primary.service';
import { Login } from '../Interface/Login';
import { NgModel } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import {  LoginResponse } from '../Interface/LoginResponse';
import { PrivateService } from '../services/Hub/private.service';
import { ToastrService } from 'ngx-toastr';


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  errorMessage: string =""

  constructor(private primary:PrimaryService, private oneone:PrivateService,private toastr: ToastrService){

  }

  email : string="";
  password : string="";
    
  
  
  Login() {
    const login: Login = {
      email: this.email,
      password: this.password,
      // Assign values for other properties as needed
    };

    this.primary.login(login).subscribe({
      next: (response:LoginResponse) => {
        if(response.statusCode=="200"){
            var jsonString = JSON.stringify(response.identity);
            localStorage.setItem('userData', jsonString);
            this.primary.isLoggedin = true;
            this.primary.identity = response.identity;
            this.oneone.onConnect();
            this.toastr.success('Login Successful', 'Login');
        }

        //this.oneone.CallMe();

        //console.log(this.primary.identity);
        //if(response.statusCode!=200)throw error;
        // Assuming response contains credentials or any other success data
        if(response.statusCode !="200"){
          this.toastr.error(response.message,'Login');
        }
      },
      error: (error: HttpErrorResponse) => {
        console.error(error); // Log the error object to console
        if (error.error) {
          const errorResponse: LoginResponse = error.error;
          console.log(`Status Code: ${errorResponse.statusCode}, Status: ${errorResponse.status}, Message: ${errorResponse.message}`);
          this.errorMessage = errorResponse.message || 'An error occurred.'; // Display error message to user
        } else {
          this.errorMessage = 'An unexpected error occurred.';
        }
      }
    });

    // Reset email and password after login attempt
    this.email = '';
    this.password = '';
  }

}
