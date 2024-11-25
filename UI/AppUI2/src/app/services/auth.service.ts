import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environment/environment.development';
import { Constant } from '../constant/constant';
import { LoginModel, RegisterModel } from '../model/model';
import { IRegisterModel } from '../model/interface';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  constructor(private http: HttpClient) {}

  onLogin(loginData: LoginModel): Observable<any> {
    return this.http.post<any>(
      environment.API_URl + Constant.API_METHOD.AUTH.LOGIN,
      loginData
    );
  }

  onRegister(
    username: string,
    password: string,
    email: string
  ): Observable<any> {
    //const reg = new RegisterModel(registerModel.get('username').value,registerModel.get('password').value,registerModel.get('email').value);
    return this.http.post<any>(
      environment.API_URl + Constant.API_METHOD.AUTH.REGISTER,{username, password, email});
  }

  onLogout(Id: string): Observable<any> {
    return this.http.post<any>(
      environment.API_URl +
        Constant.API_METHOD.AUTH.LOGOUT +
        '?userId=' +
        `${Id}`,
      {}
    );
  }

  refreshToken(): Observable<any> {
    return this.http.post<any>(environment.API_URl + Constant.API_METHOD, {});
  }

  getRoles(): Observable<any> {
    return this.http.get<any>(
      environment.API_URl + Constant.API_METHOD.ROLE.GET_ALL
    );
  }
}
