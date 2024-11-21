import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environment/environment.development';
import { Constant } from '../constant/constant';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http: HttpClient) { }

  onLogin(): Observable<any>{
    return this.http.post<any>(environment.API_URl + Constant.API_METHOD,{});
  }

  onRegister(): Observable<any>{
    return this.http.post<any>(environment.API_URl + Constant.API_METHOD,{});
  }

  onLogut(): Observable<any>{
    return this.http.post<any>(environment.API_URl + Constant.API_METHOD,{});
  }

  refreshToken(): Observable<any>{
    return this.http.post<any>(environment.API_URl + Constant.API_METHOD,{});
  }
}
