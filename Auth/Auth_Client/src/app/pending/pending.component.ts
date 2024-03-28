import { Component, OnInit, inject } from '@angular/core';
import { pending } from '../Interface/FriendShip/Pending';
import { FriendServiceService } from '../services/friend-service.service';
import { PrimaryService } from '../services/primary.service';

@Component({
  selector: 'app-pending',
  templateUrl: './pending.component.html',
  styleUrl: './pending.component.css'
})
export class PendingComponent implements OnInit {
  
  /**
   *
   */
  constructor(private friendService: FriendServiceService) {
    
  }
  pendingRequests: pending[] = [];
  primary = inject(PrimaryService);
  ngOnInit(): void {
    this.friendService.getAllfriend(9).subscribe({
      next : response =>{
        this.pendingRequests = [...response];
      },
      error:err =>{
        console.log(err);
      }
    })
  }
  




}
