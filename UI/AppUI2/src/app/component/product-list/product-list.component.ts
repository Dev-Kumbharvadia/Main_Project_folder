import { Component, inject, OnInit } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { ProductService } from '../../services/product.service';
import { Filter, Product } from '../../model/model';
import { filter } from 'rxjs';

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [],
  templateUrl: './product-list.component.html',
  styleUrl: './product-list.component.css',
})
export class ProductListComponent implements OnInit {

  products: Product[] = [];
  router = inject(Router);
  productService = inject(ProductService);
  filter = new Filter();
currentPage: any;
totalPages: any;

  ngOnInit(): void {
    this.getSortedProductsBySellerId();
  }

  nextPage() {
    this.filter.Page+=1;
    this.productService.getSortedProductsBySellerId(this.filter).subscribe((res: any) => {
      this.products = res.data.result;
      this.currentPage = res.data.currentPage;
      this.totalPages = res.data.totalPages;
    });
    }

    previousPage() {
      this.filter.Page-=1;
      this.productService.getSortedProductsBySellerId(this.filter).subscribe((res: any) => {
        this.products = res.data.result;
        this.currentPage = res.data.currentPage;
        this.totalPages = res.data.totalPages;
      });
      }

      getSortedProductsBySellerId() {
    this.productService.getSortedProductsBySellerId(this.filter).subscribe((res: any) => {
      this.products = res.data.result;
      this.currentPage = res.data.currentPage;
      this.totalPages = res.data.totalPages;
    });
  }

  editProduct(productId: string): void {
    this.productService.updateProdId = productId;
    this.router.navigateByUrl('layout/update-product').finally();
  }

  deleteProduct(productId: string): void {
    if (confirm('Are you sure you want to delete this product?')) {
      this.productService.deleteProduct(productId).subscribe((res: any) => {
        alert(res.message);
        this.products = this.products.filter(
          (product) => product.productId !== productId
        );
      });
    }
  }
}
