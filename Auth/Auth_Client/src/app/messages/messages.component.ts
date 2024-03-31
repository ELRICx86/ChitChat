import { Component, OnInit } from '@angular/core';
import { SharedService } from '../services/shared.service';
import { ministatement } from '../Interface/MiniStatement';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrl: './messages.component.css'
})


export class MessagesComponent implements OnInit{
  
mini: ministatement | any;
  constructor(private _shared:SharedService){

  }
  ngOnInit(): void {
    
  this.mini = this._shared.getData();
    //throw new Error('Method not implemented.');
  }



}
