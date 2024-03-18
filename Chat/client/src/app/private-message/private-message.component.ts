import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { Message } from '../Models/message';
import { ChatService } from '../services/chat.service';

@Component({
  selector: 'app-private-message',
  templateUrl: './private-message.component.html',
  styleUrl: './private-message.component.css'
})
export class PrivateMessageComponent implements OnInit,OnDestroy {

  /**
   *
   */
  constructor(public chatService: ChatService) {
    
    
  }
  ngOnDestroy(): void {
    //this.chatService.stopPrivateConnection();
  }
  @Input() Private: Message[] =[]

  ngOnInit(): void {
    //this.chatService.onPrivateConnection();
  }

}
