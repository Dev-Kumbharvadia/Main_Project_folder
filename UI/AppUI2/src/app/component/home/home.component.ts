import { Component, inject, OnInit } from '@angular/core';
import { CartItem, Filter, Product } from '../../model/model';
import { HttpClient } from '@angular/common/http';
import { ProductService } from '../../services/product.service';
import { DatePipe } from '@angular/common';
import { ICartItem } from '../../model/interface';
import { CartService } from '../../services/cart.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [DatePipe, FormsModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit {
  products: Product[] = [];
  http = inject(HttpClient);
  productService = inject(ProductService);
  cartService = inject(CartService);
  filter = new Filter();
  currentPage: number = 0;
  totalPages: number = 0;
  searchValue: string = '';
  pageSize: number = 15;

  ngOnInit(): void {
    this.getSortedProducts();
    this.products = this.productService.productsList;
    this.cartService.cartItems = [];
  }

  onPageSizeChange(event: Event): void {
    const selectElement = event.target as HTMLSelectElement;
    this.pageSize = +selectElement.value;
    console.log('Page size changed to:', this.pageSize);
    this.getSortedProducts();
  }

  nextPage() {
    this.filter.Page+=1;
    this.productService.getSortedProducts(this.filter).subscribe((res: any) => {
      this.products = res.data.result;
      this.currentPage = res.data.currentPage;
      this.totalPages = res.data.totalPages;
    });
    }

    previousPage() {
      this.filter.Page-=1;
      this.productService.getSortedProducts(this.filter).subscribe((res: any) => {
        this.products = res.data.result;
        this.currentPage = res.data.currentPage;
        this.totalPages = res.data.totalPages;
      });
      }

      searchProduct() {
        this.filter.Filters = this.searchValue;
        this.productService.getSortedProducts(this.filter).subscribe((res: any) => {
          if(res.data == null){
            alert("no porduct found for search querry: " +this.searchValue);
          }
          else{
            this.products = res.data.result;
            this.currentPage = res.data.currentPage;
            this.totalPages = res.data.totalPages;
          }
        });
        }

  getAllProducts(){
    this.productService.getAllProducts().subscribe((res: any)=>{
      this.productService.productsList = res;
    })
  }

  getSortedProducts(){
    this.filter.PageSize = this.pageSize;
    this.productService.getSortedProducts(this.filter).subscribe((res: any)=>{
      this.products = res.data.result;
      this.currentPage = res.data.currentPage;
      this.totalPages = res.data.totalPages;
    })
  }

  addToCart(
    productId: string,
    productName: string,
    description: string | undefined,
    imageContent: Uint8Array,
    price: number,
    sellerName: string,
    stockQuantity: number
  ) {
    var cartQuantity = 1;
    const product: ICartItem = {
      productId,
      productName,
      description,
      imageContent,
      price,
      cartQuantity,
      sellerName,
      stockQuantity
    };
    this.cartService.addToCart(product);

  }



}
