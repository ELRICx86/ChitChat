import { Component, OnInit } from '@angular/core';
import { pending } from '../Interface/FriendShip/Pending';
import { FriendServiceService } from '../services/friend-service.service';
import { response } from 'express';
import { error } from 'console';

@Component({
  selector: 'app-friendlist',
  templateUrl: './friendlist.component.html',
  styleUrl: './friendlist.component.css'
})
export class FriendlistComponent implements OnInit {
  friends: pending[] = []

  ngOnInit(): void {
    this.friendServ.GetAllRequest(9).subscribe({
      next: (response) =>{
        this.friends = response;
      },
      error:err =>{
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
