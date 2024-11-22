import { Component, inject, OnInit } from '@angular/core';
import { IRegisterModel } from '../../model/interface';
import { RegisterModel, Role } from '../../model/model';
import { AuthService } from '../../services/auth.service';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent implements OnInit {
  registerModel: IRegisterModel = new RegisterModel();
  authService = inject(AuthService)
  roles: Role[] = [];

  registerForm = new FormGroup({
    username: new FormControl(),
    password: new FormControl(),
    confirmPassword: new FormControl(),
    email: new FormControl(),
    roleId: new FormControl(),
  });

  ngOnInit(): void {
    this.loadRoles();
  }

  onRegister()
  {
    this.authService.onRegister(this.registerModel).subscribe((res:any)=>{
      console.log(res);
    });
  }

  loadRoles(){
    this.authService.getRoles().subscribe((res:any)=>{
      this.roles = res;
    });
  }
}
