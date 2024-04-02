import { Component } from '@angular/core';
import { PrimaryService } from '../services/primary.service';
import { PrivateService } from '../services/Hub/private.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent {

  /**
   *
   */
  constructor(private primary: PrimaryService, private temp : PrivateService, private toastr: ToastrService) {
    
    
  }
LogOut() {
  this.primary.logout().subscribe({
    next:(response) => {
      this.primary.isLoggedin=false;
      this.temp.chatConnection?.invoke("HelloWorld");
      this.toastr.success(response.message,"Logout");
      console.log(response)
    },
    error: e => {console.log(e.message)
    this.toastr.error(e.message,"Logout");}
  });
}

  isNavbarOpen = false;
  isDropdownOpen = false;

  toggleNavbar() {
    this.isNavbarOpen = !this.isNavbarOpen;
  }

  toggleDropdown() {
    this.isDropdownOpen = !this.isDropdownOpen;
  }
}
