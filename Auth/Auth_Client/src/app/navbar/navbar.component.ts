import { Component } from '@angular/core';
import { PrimaryService } from '../services/primary.service';
import { PrivateService } from '../services/Hub/private.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent {

  /**
   *
   */
  constructor(private primary: PrimaryService, private temp : PrivateService) {
    
    
  }
LogOut() {
  this.primary.logout().subscribe({
    next:(response) => {
      this.primary.isLoggedin=false;
      this.temp.chatConnection?.invoke("HelloWorld");
      console.log(response)
    },
    error: e => console.log(e.message)
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
