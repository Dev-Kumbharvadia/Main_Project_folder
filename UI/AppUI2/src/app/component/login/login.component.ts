import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  
  router = inject(Router);
  
  redirectToHome(){
    this.router.navigateByUrl('http://localhost:4200/layout');
  }
}
