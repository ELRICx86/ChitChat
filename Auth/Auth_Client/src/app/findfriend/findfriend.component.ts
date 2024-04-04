import { Component, OnInit } from '@angular/core';
import { ministatement } from '../Interface/MiniStatement';
import { FriendServiceService } from '../services/friend-service.service';
import { response } from 'express';
import { potentialFriends } from '../Interface/PotentialFriends';

@Component({
  selector: 'app-findfriend',
  templateUrl: './findfriend.component.html',
  styleUrl: './findfriend.component.css'
})
export class FindfriendComponent implements OnInit {
  products: ministatement[] =[];


    responsiveOptions: any[] | undefined;
    Friends:potentialFriends[] =[];
    selectedfriend: any;

    constructor(private friend: FriendServiceService) {}

    ngOnInit() {
        this.friend.getAllfriend(9).subscribe({
          next:response=>{
            //console.log(response);
            this.products = response
          },
          error:err =>{
            console.log(err)
          }
        });

        this.responsiveOptions = [
          {
              breakpoint: '1199px',
              numVisible: 2,
              numScroll: 1
          },
          {
              breakpoint: '991px',
              numVisible: 2,
              numScroll: 1
          },
          {
              breakpoint: '767px',
              numVisible: 2,
              numScroll: 1
          }
      ];

    }
        
       

    

}
