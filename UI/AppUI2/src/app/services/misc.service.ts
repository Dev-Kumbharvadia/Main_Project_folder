import { HttpClient } from '@angular/common/http';
import { ENVIRONMENT_INITIALIZER, inject, Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { environment } from '../../environment/environment.development';
import { Constant } from '../constant/constant';

@Injectable({
  providedIn: 'root'
})
export class MiscService {

  http = inject(HttpClient);

  constructor() { }

  setCookie(name: string, value: string, minutes: number): void {
    const date = new Date();
    date.setTime(date.getTime() + minutes * 60 * 1000);
    const expires = `expires=${date.toUTCString()}`;
    document.cookie = `${name}=${value}; ${expires}; path=/`;
  }

  getUserRoles():  Observable<any>{
    var Id: string = sessionStorage.getItem('userId') ?? '';
    return this.http.get(environment.API_URl + Constant.API_METHOD.ROLE.GET_BY_ID + "?userId=" + `${Id}`);
  }

  loadSalesData(){
    return this.http.get(environment.API_URl + Constant.API_METHOD.SELLER.BY_ID + `?sellerId=${sessionStorage.getItem('userId')}`);
  }
}
