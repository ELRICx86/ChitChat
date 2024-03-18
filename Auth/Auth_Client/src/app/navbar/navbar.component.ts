import { Component } from '@angular/core';
import { PrimaryService } from '../services/primary.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent {

  /**
   *
   */
  constructor(private primary: PrimaryService) {
    
    
  }
LogOut() {
  this.primary.logout().subscribe({
    next:(response) => console.log(response),
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
