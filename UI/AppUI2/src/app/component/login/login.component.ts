import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import { ILoginModel } from '../../model/interface';
import { LoginModel } from '../../model/model';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { MiscService } from '../../services/misc.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  
  authServices = inject(AuthService);
  router = inject(Router);
  loginData: ILoginModel = new LoginModel(); 
  miscServices = inject(MiscService);

  onLogin(){
    this.authServices.onLogin(this.loginData).subscribe((res: any)=>{
      this.miscServices.setCookie('jwtToken',res.data.jwtToken,1)
      this.miscServices.setCookie('refreshToken',res.data.refreshToken,7);
      sessionStorage.setItem('userId',res.data.userId);
    });
    this.router.navigateByUrl('layout/home');
  }
}
