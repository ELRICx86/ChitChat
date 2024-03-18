import { Component, OnInit, Output, EventEmitter, OnDestroy } from '@angular/core';
import { ChatService } from '../services/chat.service';
import { Message } from '../Models/message';


@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrl: './chat.component.css'
})
export class ChatComponent implements OnInit,OnDestroy {
  title: string = "Global Chat";
  message : Message | undefined ;
  clicked: boolean=false;



      childClick(user: string) {
      this.clicked =!this.clicked;
      if(this.clicked){
        this.title = user;
      }
      else this.title = "Global Chat"
    }


sendMessage(content:string) {
  if(!this.clicked){
  this.chatService.sendMessage(content);
  }
  else{
    this.chatService.sendPrivateMessage(this.title, content);
  }
}

  /**
   *
   */
  constructor(public chatService : ChatService) {
    
    
  }
  ngOnDestroy(): void {
    this.chatService.stopChatConnection();
    //console.log(this.chatService.onlineUsers);
  }
  @Output() logoutEmmiter = new EventEmitter<any> ();
  
  ngOnInit(): void {
    this.chatService.createChatConnection();
    // if(this.clicked){
    //   this.title = "send msg to"
    // }
  }


  Logout(){
    this.logoutEmmiter.emit();
  }
}
