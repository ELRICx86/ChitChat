import { Component, Input, OnInit, inject, input } from '@angular/core';
import { FriendServiceService } from '../services/friend-service.service';
import { ministatement } from '../Interface/MiniStatement';
import { response } from 'express';
import { error, log } from 'console';
import { PrimaryService } from '../services/primary.service';

@Component({
  selector: 'app-contacts',
  templateUrl: './contacts.component.html',
  styleUrl: './contacts.component.css'
})
export class ContactsComponent implements OnInit {

  //randomColor: string;

  // filteredList: ministatement[] = [];
  // @Input() child_searchTerm: string = '';
  
  contacts: ministatement[] = []

  constructor(private friendServ: FriendServiceService) {
  }

  // primary = inject(PrimaryService);

  // filterList(): void {
  //   if (this.child_searchTerm.trim() !== '') {
  //     this.filteredList = this.contacts.filter(item =>
  //       item.firstName.toLowerCase().includes(this.child_searchTerm.toLowerCase())
  //     );
  //   } else {
  //     // If search term is empty, display the original list
  //     this.filteredList = this.contacts;
  //   }
  // }


  
  ngOnInit(): void {
    this.friendServ.getMiniStatement(9).subscribe({
      next : response =>{
        //console.log(response);
        this.contacts = response;
      },
      error: error =>{
        console.log(error);
      }
    })
  }


  getRandomColor(name: string): string {
    const colors: string[] = [
      '#FF5733', // Red
      '#FFBD33', // Orange
      '#FFD133', // Yellow
      '#FF33BE', // Pink
      '#FF33E8', // Purple
      '#8E33FF', // Violet
      '#3361FF', // Blue
      '#33A4FF', // Sky Blue
      '#33FFF9', // Cyan
      '#33FF6E', // Mint
      '#33FF33', // Green
      '#3FFF33', // Lime Green
      '#7AFF33', // Spring Green
      '#B5FF33', // Chartreuse Green
      '#DFFF33', // Yellow Green
      '#FFD033', // Gold
      '#FF9C33', // Orange
      '#FF7F33', // Tangerine
      '#FF5733', // Coral
      '#E83333', // Crimson
      '#D033FF', // Lavender
      '#7533FF', // Indigo
      '#3333FF', // Royal Blue
      '#33E5FF', // Turquoise
      '#33FFA1'  // Aquamarine
  ];
  
  
    // Get the character code of the first character of the name
    const charCode = name.charCodeAt(0);
    // Calculate the index based on the character code
    const index = charCode % colors.length;
    // Return the color corresponding to the index
    return colors[index];
  }
}
