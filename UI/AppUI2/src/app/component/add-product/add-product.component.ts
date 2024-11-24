import { Component, inject } from '@angular/core';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ProductService } from '../../services/product.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-add-product',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './add-product.component.html',
  styleUrl: './add-product.component.css'
})
export class AddProductComponent {

  productService = inject(ProductService);
  router = inject(Router);

  product = {
    productName: '',
    description: '',
    image: null as File | null,
    price: 0,
    stockQuantity: 0,
    sellerId: sessionStorage.getItem('userId') || ''
  };

  constructor() {}

  onFileChange(event: any) {
    const file = event.target.files[0];
    if (file) {
      this.product.image = file;
    }
  }

  onSubmit() {
    const formData = new FormData();

    if (this.product.productName) {
      formData.append('productName', this.product.productName);
    } else {
      alert('Product name is required');
      return;
    }

    formData.append('description', this.product.description || '');

    if (this.product.price > 0) {
      formData.append('price', this.product.price.toString());
    } else {
      alert('Price must be greater than zero');
      return;
    }

    if (this.product.stockQuantity >= 0) {
      formData.append('stockQuantity', this.product.stockQuantity.toString());
    } else {
      alert('Stock quantity must be zero or greater');
      return;
    }

    if (this.product.sellerId) {
      formData.append('sellerId', this.product.sellerId);
    } else {
      alert('Seller ID is missing');
      return;
    }

    if (this.product.image) {
      formData.append('imageFile', this.product.image, this.product.image.name);
    } else {
      alert('No image file selected');
      return;
    }

    this.productService.addProduct(formData).subscribe(
      (res: any) => {
        alert('Product Added');
        this.resetProduct();
      },
      (error) => {
        console.error('Error submitting product:', error);
      }
    );
    this.resetProduct();
  };

  resetProduct(){
    this.product = {
      productName: '',
      description: '',
      image: null as File | null,
      price: 0,
      stockQuantity: 0,
      sellerId: sessionStorage.getItem('userId') || ''
    };
  }
}
