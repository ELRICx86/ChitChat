import { Component, TemplateRef, inject } from '@angular/core';
import { PrimaryService } from '../services/primary.service';
import { Register } from '../Interface/Register';
import { response } from 'express';
import { Login } from '../Interface/Login';
import { ModalDismissReasons, NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {

  constructor(private primary: PrimaryService){

  }
  private modalService = inject(NgbModal);

  closeResult = '';



  firstname: string ="";
  lastname: string ="";
  username: string ="";
  email: string ="";
  password: string ="";
  confirmPassword: string ="";


  Register() {
    const register: Register = {
      firstname: this.firstname,
      lastname: this.lastname,
      username: this.username,
      email: this.email,
      password: this.password,
      confirmpassword: this.confirmPassword
    };
  
    if (this.password == this.confirmPassword) {
      this.primary.register(register).subscribe({
        next: (response: Register) => {
          console.log(response);
        },
        error: (error) => {
          console.log(error);
        }
      });
    }
    this.firstname ="";
    this.lastname ="";
    this.username ="";
    this.email ="";
    this.password ="";
    this.confirmPassword ="";
  }
  
}
