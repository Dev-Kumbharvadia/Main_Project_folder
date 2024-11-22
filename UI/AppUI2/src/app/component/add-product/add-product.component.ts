import { Component, inject } from '@angular/core';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ProductService } from '../../services/product.service';

@Component({
  selector: 'app-add-product',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './add-product.component.html',
  styleUrl: './add-product.component.css'
})
export class AddProductComponent {
  
  productService = inject(ProductService);

  // Define the product model with default values
  product = {
    productName: '',
    description: '',
    image: null as File | null,
    price: 0,
    stockQuantity: 0,
    sellerId: sessionStorage.getItem('userId') || ''
  };

  constructor() {}

  // Method to handle the file input change
  onFileChange(event: any) {
    const file = event.target.files[0];
    if (file) {
      this.product.image = file; // Assign the selected file to the image property
    }
  }

  // Method to handle the form submission
  onSubmit() {
    // Create FormData instance to handle file upload
    const formData = new FormData();
    
    // Append each form field to FormData
    formData.append('productName', this.product.productName);
    formData.append('description', this.product.description || '');
    formData.append('price', this.product.price.toString());
    formData.append('stockQuantity', this.product.stockQuantity.toString());
    formData.append('sellerId', this.product.sellerId);

    // Handle the image file if it exists
    if (this.product.image) {
      formData.append('imageFile', this.product.image, this.product.image.name); // Append the image file
    } else {
      console.log('No image file selected');
    }

    // Submit the form data to the ProductService
    this.productService.addProduct(formData).subscribe(
      (res: any) => {
        console.log(res); // Handle success
      },
      (error) => {
        console.error('Error submitting product:', error); // Handle error
      }
    );
  }





  // addProductForm = new FormGroup({
  //   productName: new FormControl(''),
  //   description: new FormControl(''),
  //   image: new FormControl<File | null>(null),
  //   price: new FormControl(''),
  //   stockQuantity: new FormControl(''),
  //   sellerId: new FormControl(''),
  // });

  // onSubmit() {
  //   const sellerId = sessionStorage.getItem('userId');
  
  //   if (sellerId) {
  //     this.addProductForm.patchValue({
  //       sellerId: sellerId // Assuming sellerId is in string format
  //     });
  //   } else {
  //     console.log('User ID not found in sessionStorage');
  //     return; // Exit if no sellerId is found
  //   }
  
  //   // Create FormData instance to handle file upload
  //   const formData = new FormData();
  //   formData.append('productName', this.addProductForm.get('productName')?.value || '');
  //   formData.append('description', this.addProductForm.get('description')?.value || '');
  //   formData.append('price', this.addProductForm.get('price')?.value || '');
  //   formData.append('stockQuantity', this.addProductForm.get('stockQuantity')?.value || '');
  //   formData.append('sellerId', this.addProductForm.get('sellerId')?.value || '');
  
  //   // Handle the image file if it exists
  //   const imageFile = this.addProductForm.get('image')?.value;
  //   if (imageFile && imageFile instanceof File) { // Check if it's a valid file object
  //     formData.append('imageFile', imageFile, imageFile.name);
  //   } else {
  //     console.log('No image file selected');
  //   }
  
  //   console.log(formData);
  //   // Submit the form data
  //   this.productService.addProduct(formData).subscribe((res: any) => {
  //     console.log(res);
  //   }, (error) => {
  //     console.error('Error submitting product:', error);
  //   });
  // }
  
  
}
