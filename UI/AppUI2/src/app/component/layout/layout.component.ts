import { Component, inject, OnInit } from '@angular/core';
import { Router, RouterLink, RouterOutlet } from '@angular/router';
import { CartComponent } from '../cart/cart.component';
import { AuthService } from '../../services/auth.service';
import { MiscService } from '../../services/misc.service';

@Component({
  selector: 'app-layout',
  standalone: true,
  imports: [RouterOutlet, RouterLink],
  templateUrl: './layout.component.html',
  styleUrl: './layout.component.css',
})
export class LayoutComponent implements OnInit {
  authService = inject(AuthService);
  miscService = inject(MiscService);
  router = inject(Router);
  userRoles: string[] = [];

  ngOnInit(): void {
    this.getUserRoles();
  }

  getUserRoles() {
    this.miscService.getUserRoles().subscribe((res: any) => {
      this.userRoles = res.data;  
    });
  }

  onLogout() {
    var Id = sessionStorage.getItem('userId') ?? '';
    this.authService.onLogout(Id).subscribe((res: any) => {
      sessionStorage.clear();
      localStorage.clear();
      this.clearCookies();
      this.router.navigateByUrl('');
    });
  }

  clearCookies() {
    const cookies = document.cookie.split(';');
    for (let cookie of cookies) {
      const eqPos = cookie.indexOf('=');
      const name = eqPos > -1 ? cookie.substr(0, eqPos) : cookie;
      document.cookie = name + '=;expires=Thu, 01 Jan 1970 00:00:00 GMT;path=/';
    }
  }
}
