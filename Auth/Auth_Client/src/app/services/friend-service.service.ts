import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class FriendServiceService {

  apiUrl ="http://localhost:5285/friend";

  constructor(private http : HttpClient) {

   }

   getAllfriend(UserId :any):Observable<any> {
     return this.http.get<any>(`${this.apiUrl}/getall/${UserId}`, {withCredentials:true});
  }

  GetFriendById(UserId:number):Observable<any>{
    return this.http.get<any>(`${this.apiUrl}/get/${UserId}`,{withCredentials:true})
  }

  AddFriend(user:number, friend:number):Observable<any>{
   

    // Prepare the query parameters
    let params = new HttpParams();
    params = params.append('user', user);
    params = params.append('friend', friend);

    // Make the POST request with query parameters in the URL
    return this.http.post<any>(`${this.apiUrl}/add`, {}, { params: params ,withCredentials:true});
  }

  GetAllRequest(UserId: any):Observable<any>{
    return this.http.get<any>(`${this.apiUrl}/request/${UserId}`,{withCredentials:true});
  }

  GetAllPendings(UserId: any):Observable<any>{
    return this.http.get<any>(`${this.apiUrl}/pendings/${UserId}`,{withCredentials:true});
  }

  RemoveFriend(user:number, friend:number):Observable<any>{
    // Prepare the query parameters
    let params = new HttpParams();
    params = params.append('user', user);
    params = params.append('friend', friend);

    // Make the POST request with query parameters in the URL
    return this.http.delete<any>(`${this.apiUrl}/remove`, { params: params ,withCredentials:true});
  }

  Response(user: number, friend: number, action: string): Observable<any> {
    // Prepare the query parameters
    let params = new HttpParams();
    params = params.append('user', user.toString());
    params = params.append('friend', friend.toString());
    params = params.append('action', action);

    // Make the POST request with query parameters
    return this.http.post<any>(`${this.apiUrl}/response`, {}, { params: params, withCredentials: true });
  }

  getMiniStatement(userId: any): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/ministatement/${userId}`, { withCredentials: true });
  }


  
}
  

