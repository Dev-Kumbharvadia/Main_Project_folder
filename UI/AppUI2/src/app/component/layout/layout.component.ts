import { Component, inject } from '@angular/core';
import { Router, RouterLink, RouterOutlet } from '@angular/router';
import { CartComponent } from '../cart/cart.component';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-layout',
  standalone: true,
  imports: [RouterOutlet, RouterLink],
  templateUrl: './layout.component.html',
  styleUrl: './layout.component.css'
})
export class LayoutComponent {
  userRole:any;
  authService = inject(AuthService);
  router = inject(Router);

  onLogout(){
    var Id = sessionStorage.getItem('userId') ?? '';
    this.authService.onLogut(Id).subscribe((res: any)=>{
      this.router.navigateByUrl('');
      
    });
  }

}
