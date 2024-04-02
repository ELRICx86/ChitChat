import { Component, OnInit, inject } from '@angular/core';
import { pending } from '../Interface/FriendShip/Pending';
import { FriendServiceService } from '../services/friend-service.service';
import { response } from 'express';
import { error } from 'console';
import { PrimaryService } from '../services/primary.service';

@Component({
  selector: 'app-friendlist',
  templateUrl: './friendlist.component.html',
  styleUrl: './friendlist.component.css'
})
export class FriendlistComponent implements OnInit {
  friends: pending[] = []
  primary = inject(PrimaryService);
  ngOnInit(): void {
    this.friendServ.GetAllRequest(this.primary.getUserInfo().userId).subscribe({
      next: (response) =>{
        this.friends = response;
      },
      error:err =>{
        //this.primary.isLoggedin = false;
        console.log(err)
      }
    })
  }

  /**
   *
   */
  constructor(private friendServ:FriendServiceService) {
    
    
  }
  
  

}
