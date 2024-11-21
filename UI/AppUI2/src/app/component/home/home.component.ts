import { Component } from '@angular/core';
import { Product } from '../../model/model';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {
  Products: Product[] = [];

  
}
