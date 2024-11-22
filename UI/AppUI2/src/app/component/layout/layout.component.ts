import { Component } from '@angular/core';
import { RouterLink, RouterOutlet } from '@angular/router';
import { CartComponent } from '../cart/cart.component';

@Component({
  selector: 'app-layout',
  standalone: true,
  imports: [RouterOutlet, RouterLink],
  templateUrl: './layout.component.html',
  styleUrl: './layout.component.css'
})
export class LayoutComponent {
  userRole:any;

}
