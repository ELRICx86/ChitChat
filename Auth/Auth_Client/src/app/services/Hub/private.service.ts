import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder, HubConnectionState } from '@microsoft/signalr';
import { error } from 'console';

import { ministatement } from '../../Interface/MiniStatement';
import { Observable } from 'rxjs';
import { HttpParams,HttpClient } from '@angular/common/http';
import { Connection } from '../../Interface/Connection';
import { PrimaryService } from '../primary.service';

@Injectable({
  providedIn: 'root'
})
export class PrivateService {

  public chatConnection? : HubConnection;
  constructor(private http:HttpClient, private primary:PrimaryService) { }

  FriendConnectionList : Connection [] = [];


  

   onConnect() {
   
      this.chatConnection = new HubConnectionBuilder().withUrl('http://localhost:5285/private').withAutomaticReconnect().build();
      this.chatConnection.start()
        .then(() => 
          {console.log('Connection established successfully')})
           .then(()=>{
            //this.CallMe();
          })
        .catch(error => {
          console.error('Error while establishing connection:', error);
          // Reject the Prompise if connection fails
        });

        this.chatConnection?.on("OnConnect",(message:string) =>{

          const temp: Connection={
            connectionId : message,
            userId : this.primary.identity?.userId
          };


          this.chatConnection?.invoke("Connected", temp).catch(error => console.log(error)); 
          console.log("Your connection Id is :"+ message);
        });


        
        this.chatConnection?.on("UserConnected", (conn: Connection) =>{
          this.FriendConnectionList.push(conn);
          console.log(conn.userId +"with connection id" + conn.connectionId);
        });


    }
  

  public CallMe() {
    if (!this.chatConnection) {
      console.error('Chat connection is not initialized.');
      return;
    }

    try {
       //this.chatConnection.invoke("getToken");
      console.log('getToken method invoked');
    } catch (err) {
      console.error('Error while invoking getToken method:', err);
    }
  }

  getMessage(from: number, to: number, miniStatement: ministatement): Observable<any> {
    
    const message = {
      To:" ",
      From:" ",
      Content: " "
      
    };


    const params = new HttpParams()
      .set('from', from.toString())
      .set('to', to.toString());

    const url = 'http://localhost:5285/message';

    // Return the Observable returned by HttpClient.post()
    return this.http.post<any[]>(url, { message }, { params });
  }


  

  
  

       
}
