import { inject, Injectable } from '@angular/core';
import { CartItem, Product } from '../model/model';
import { ProductService } from './product.service';

@Injectable({
  providedIn: 'root'
})
export class CartService {

  constructor() { }

  productService = inject(ProductService)
  cartItems: CartItem[] = [];

  onPurchase(){

  }

  addToCart(product: CartItem) {
    const existingCartItem = this.cartItems.find(item => item.productId === product.productId);
    if (existingCartItem) {
      existingCartItem.cartQuantity += 1;
    } else {
      product.cartQuantity = 1;
      this.cartItems.push(product);
    }
    console.log(this.cartItems);
  }

  onCartUpdate(): CartItem[]{
    return this.cartItems;
  }

}
