import { Component, inject, OnInit } from '@angular/core';
import { ProductService } from '../../services/product.service';
import { CartItem, Product } from '../../model/model';
import { CartService } from '../../services/cart.service';

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [],
  templateUrl: './cart.component.html',
  styleUrl: './cart.component.css',
})
export class CartComponent implements OnInit {
  cartService = inject(CartService);
productService = inject(ProductService);
cartItems: CartItem[] = [];

ngOnInit(): void {
  this.loadAndUpdateCartData();
  console.log(this.cartItems);
}

loadAndUpdateCartData() {
  const storedCart = sessionStorage.getItem('cart');
  let cartData = storedCart ? JSON.parse(storedCart) : [];

  const updatedCartItems = this.cartService.onCartUpdate();

  if (updatedCartItems && updatedCartItems.length > 0) {
    updatedCartItems.forEach((newItem: CartItem) => {
      const existingItem = cartData.find((item: any) => item.productId === newItem.productId);
      if (existingItem) {
        existingItem.cartQuantity += newItem.cartQuantity;
      } else {
        cartData.push({
          productId: newItem.productId,
          cartQuantity: newItem.cartQuantity,
        });
      }
    });
  }

  if (cartData.length === 0) {
    console.log('No new items to update in the cart.');
  } else {
    sessionStorage.setItem('cart', JSON.stringify(cartData));
    this.cartItems = [];

    cartData.forEach((cartItem: { productId: string; cartQuantity: number }) => {
      this.productService.getProductById(cartItem.productId).subscribe((product: any) => {
        this.cartItems.push({
          ...product,
          cartQuantity: cartItem.cartQuantity,
        });
      });
    });
  }
}


getProductById(Id: string) {
  this.productService.getProductById(Id).subscribe((res: any) => {
    this.cartItems.push(res);
  });
}

addItem(Id: string) {
  const product = this.cartItems.find((item) => item.productId === Id);
  if (product) {
    product.cartQuantity += 1;
    this.updateSessionStorage();
  }
}

subItem(Id: string) {
  const product = this.cartItems.find((item) => item.productId === Id);
  if (product) {
    if (product.cartQuantity > 1) {
      product.cartQuantity -= 1;
      this.updateSessionStorage();
    }
  }
}

private updateSessionStorage() {
  const cartData = this.cartItems.map((item) => ({
    productId: item.productId,
    cartQuantity: item.cartQuantity,
  }));

  sessionStorage.setItem('cart', JSON.stringify(cartData));
}

removeItem(Id: string) {

  this.cartItems = this.cartItems.filter((item) => item.productId !== Id);

  const storedCart = sessionStorage.getItem('cart');

  let cartData = storedCart ? JSON.parse(storedCart) : [];

  cartData = cartData.filter((item: any) => item.productId !== Id);

  sessionStorage.setItem('cart', JSON.stringify(cartData));
}

}
