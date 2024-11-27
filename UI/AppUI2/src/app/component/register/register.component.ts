import { Component, inject, OnInit } from '@angular/core';
import { IRegisterModel } from '../../model/interface';
import { RegisterModel, Role } from '../../model/model';
import { AuthService } from '../../services/auth.service';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { AdminService } from '../../services/admin.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css',
})
export class RegisterComponent implements OnInit {
  authService = inject(AuthService);
  adminService = inject(AdminService);
  roles: Role[] = [];
  router = inject(Router)

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

  onRegister(): void {
    if (this.registerForm.invalid) {
      alert('Form is invalid');
      return;
    }
    var userId: string;

    if(this.registerForm.get('password')?.value != this.registerForm.get('confirmPassword')?.value){
      alert("password and confirm password dont match")
      return;
    }

    this.authService
      .onRegister(
        this.registerForm.get('username')?.value,
        this.registerForm.get('password')?.value,
        this.registerForm.get('email')?.value
      )
      .subscribe((res: any) => {
        userId = res.data?.userId;
        if(res.success == false){
          alert(res.message)
        } else {
          this.adminService
            .assignRole(userId, this.registerForm.get('roleId')?.value)
            .subscribe((roleRes: any) => {
              console.log('Role assigned:', roleRes);
            });
        }
      });
      this.router.navigateByUrl('login');
  }

  loadRoles() {
    this.authService.getRoles().subscribe((res: any) => {
      this.roles = res;
    });
  }
}
