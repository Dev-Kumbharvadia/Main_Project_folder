import { Component, inject, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ProductService } from '../../services/product.service';
import { Filter, Product } from '../../model/model';

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [],
  templateUrl: './product-list.component.html',
  styleUrl: './product-list.component.css'
})
export class ProductListComponent implements OnInit{
  
  products: Product[] = [];
  router = inject(Router)
  productService = inject(ProductService)
  filter = new Filter();

  ngOnInit(): void {
    this.getSortedProducts();
    this.products = this.productService.productsList;
  }

  getSortedProducts(){
    this.productService.getSortedProducts(this.filter).subscribe((res: any)=>{
      this.products = res;
    })
  }
  editProduct(productId: string): void {
    this.productService.updateProdId = productId;
    this.router.navigateByUrl('layout/update-product');
  }

  deleteProduct(productId: string): void {
    if (confirm('Are you sure you want to delete this product?')) {
      this.productService.deleteProduct(productId).subscribe(() => {
        // Filter out the deleted product from the list
        this.products = this.products.filter(p => p.productId !== productId);
      });
    }
  }
    
}
