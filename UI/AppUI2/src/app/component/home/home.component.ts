import { Component, inject, OnInit } from '@angular/core';
import { CartItem, Filter, Product } from '../../model/model';
import { HttpClient } from '@angular/common/http';
import { ProductService } from '../../services/product.service';
import { DatePipe } from '@angular/common';
import { ICartItem } from '../../model/interface';
import { CartService } from '../../services/cart.service';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [DatePipe],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit {
  products: Product[] = [];
  http = inject(HttpClient);
  productService = inject(ProductService);
  cartService = inject(CartService);
  filter = new Filter();

  ngOnInit(): void {
    // this.getAllProducts()
    this.getSortedProducts();
    this.products = this.productService.productsList;
    this.cartService.cartItems = [];
  }

  getAllProducts(){
    this.productService.getAllProducts().subscribe((res: any)=>{
      this.productService.productsList = res;
    })
  }

  getSortedProducts(){
    this.productService.getSortedProducts(this.filter).subscribe((res: any)=>{
      console.log(res);
      this.products = res;
    })
  }

  addToCart(
    productId: string,
    productName: string,
    description: string | undefined,
    imageContent: Uint8Array,
    price: number,
    sellerName: string
  ) {
    var cartQuantity = 1;
    const product: ICartItem = {
      productId,
      productName,
      description,
      imageContent,
      price,
      cartQuantity,
      sellerName
    };

    // Pass the product to the service
    this.cartService.addToCart(product);

    // console.log(product); // Debug: log the product object
  }



}
