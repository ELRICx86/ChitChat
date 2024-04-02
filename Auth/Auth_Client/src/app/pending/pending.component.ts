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

    var userData = localStorage.getItem('userData');
  
    if (userData !== null) {
      var item = JSON.parse(userData);
      
      if (item && item.userId !== undefined) {
        
        
        this.friendService.GetAllPendings(item.userId).subscribe({
          next : response =>{
            this.pendingRequests = [...response];
          },
          error:err =>{
            console.log(err);
          }
        });
      } else {
        console.log("User data is incomplete.");
      }
    } else {
      console.log("No user data found in local storage.");
    }


    
  }



  RejectReq(userid:number) {
    
    this.friendService.Response(this.primary.getUserInfo().userId, userid , "reject").subscribe({
      next:response =>{
        alert(response.message);
      },
      error:err=>{
        console.log(err)
      }
    })
    }


  AcceptReq(userid:number) {
    this.friendService.Response(this.primary.getUserInfo().userId, userid , "accept").subscribe({
      next:response =>{
        alert(response.message);
      },
      error:err=>{
        console.log(err)
      }
    })
    }
}
