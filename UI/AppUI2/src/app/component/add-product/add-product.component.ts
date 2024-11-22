import { Component } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-add-product',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './add-product.component.html',
  styleUrl: './add-product.component.css'
})
export class AddProductComponent {
  addProductForm = new FormGroup({
    productName: new FormControl(''),
    description: new FormControl(''),
    imageContent: new FormControl(''),
    price: new FormControl(''),
    stockQuantity: new FormControl(''),
    sellerId: new FormControl(''),
  });

  onSubmit(){
    
  }
}
