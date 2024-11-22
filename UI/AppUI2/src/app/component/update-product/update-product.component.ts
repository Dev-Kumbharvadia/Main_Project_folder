import { Component, inject, OnInit } from '@angular/core';
import { ProductService } from '../../services/product.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-update-product',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './update-product.component.html',
  styleUrl: './update-product.component.css'
})
export class UpdateProductComponent implements OnInit {
  productService = inject(ProductService);

  // Define the product model with default values
  product = {
    productName: '',
    description: '',
    image: null as File | null,
    price: 0,
    stockQuantity: 0,
    sellerId: sessionStorage.getItem('userId') || '' // Assuming the seller ID doesn't change
  };
  imageData: string = '';
  prodId: string = '';

  constructor() {}

  ngOnInit(): void {
    // Check if the product ID is available before loading data
    this.prodId = this.productService.updateProdId;
    if (this.prodId) {
      this.loadData(this.prodId);
    } else {
      console.error('Product ID is not provided');
    }
  }

  loadData(Id: string): void {
    this.productService.getProductById(Id).subscribe(
      (res: any) => {
        // Map response data to the product object
        this.product.productName = res.productName || '';
        this.product.description = res.description || '';
        this.product.price = res.price || 0;
        this.product.stockQuantity = res.stockQuantity || 0;
        this.product.sellerId = res.sellerId || this.product.sellerId;
        this.imageData = res.imageContent
        console.log('Product data loaded:', res);
      },
      (error) => {
        console.error('Error fetching product details:', error);
      }
    );
  }

  // Method to handle the file input change
  onFileChange(event: any): void {
    const file = event.target.files[0];
    if (file) {
      this.product.image = file; // Assign the selected file to the `image` property
      console.log('File selected:', file);
    }
  }

  // Method to handle the form submission for updating a product
  onSubmit(): void {
    // Create FormData instance to handle file upload
    const formData = new FormData();

    // Append each field to FormData
    formData.append('productName', this.product.productName);
    formData.append('price', this.product.price.toString());
    formData.append('stockQuantity', this.product.stockQuantity.toString());
    formData.append('description', this.product.description || '');

    // Append the image file if it exists
    if (this.product.image) {
      formData.append('file', this.product.image, this.product.image.name);
    } else {
      console.warn('No image file selected');
    }

    // Submit the form data for updating the product
    this.productService.updateProduct(this.prodId, formData).subscribe(
      (res: any) => {
        console.log('Product updated successfully:', res);
      },
      (error) => {
        console.error('Error updating product:', error);
      }
    );
  }
  
}
