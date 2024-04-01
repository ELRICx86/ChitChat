import { Component, OnInit, inject } from '@angular/core';
import { FriendServiceService } from '../services/friend-service.service';
import { ministatement } from '../Interface/MiniStatement';
import { PrimaryService } from '../services/primary.service';
import { SharedService } from '../services/shared.service';

@Component({
  selector: 'app-contacts',
  templateUrl: './contacts.component.html',
  styleUrl: './contacts.component.css'
})
export class ContactsComponent implements OnInit {




    onSelectItem(_t5: ministatement) {
       this._shared.setData(this.selectedItem);
    }

  //randomColor: string;

  // filteredList: ministatement[] = [];
  // @Input() child_searchTerm: string = '';
  
  contacts: ministatement[] = []
  selectedItem: ministatement|undefined;

  constructor(private friendServ: FriendServiceService, private _shared:SharedService) {
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


  primary = inject(PrimaryService);
  ngOnInit(): void {
    if(this.primary.identity?.userId != null)
    localStorage.setItem("userid", String(this.primary.identity?.userId));
    var userid =  Number(localStorage.getItem("userid"));
    this.friendServ.getMiniStatement(userid).subscribe({
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
