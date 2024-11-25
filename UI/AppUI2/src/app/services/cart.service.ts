import { inject, Injectable } from '@angular/core';
import { CartItem, MakePurchase, Product } from '../model/model';
import { ProductService } from './product.service';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environment/environment.development';
import { Constant } from '../constant/constant';

@Injectable({
  providedIn: 'root'
})
export class CartService {

  constructor() { }

  http = inject(HttpClient);
  productService = inject(ProductService)
  cartItems: CartItem[] = [];

  onPurchase(obj: MakePurchase): Observable<any>{
    return this.http.post(environment.API_URl + Constant.API_METHOD.TRANSACTION.MAKE_PURCHASE,obj);
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
