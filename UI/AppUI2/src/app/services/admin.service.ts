import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../environment/environment.development';
import { Constant } from '../constant/constant';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AdminService {

  http = inject(HttpClient);

  constructor() { }

  assignRole(userId: string, roleId: string): Observable<any>{
    return this.http.post<any>(environment.API_URl + Constant.API_METHOD.USER.ASSIGN_ROLE+"?userId="+`${userId}`+"&roleId="+`${roleId}`,{});
  }

  getAllAudits(): Observable<any>{
    return this.http.get<any>(environment.API_URl + Constant.API_METHOD.AUDIT.GET_ALL);
  }

  getAllUserInfo(): Observable<any>{
    return this.http.get<any>(environment.API_URl + Constant.API_METHOD.USER.GET_ALL_INFO)
  }

  getAllUserInfoById(Id: string): Observable<any>{
    return this.http.get<any>(environment.API_URl + Constant.API_METHOD.USER.GET_ALL_INFO_BY_ID + "?id=" + `${Id}`)
  }

  getUserPurchases(Id: string): Observable<any>{
    return this.http.get<any>(environment.API_URl + Constant.API_METHOD.TRANSACTION.GET_TRANSACTION_HISTORY_BY_USER_ID + "?id=" + `${Id}`)
  }
}
