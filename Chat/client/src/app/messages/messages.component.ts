import { Component, Input, OnInit } from '@angular/core';
import { Message } from '../Models/message';
import { ChatService } from '../services/chat.service';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrl: './messages.component.css'
})
export class MessagesComponent implements OnInit {

  ngOnInit(): void {
  }

  @Input() messages: Message[] =[]



}
