import { Component, inject, OnInit } from '@angular/core';
import { ProductService } from '../../services/product.service';
import { CartItem, MakePurchase, Product } from '../../model/model';
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
  }

  loadAndUpdateCartData() {
    const storedCart = sessionStorage.getItem('cart');
    let cartData = storedCart ? JSON.parse(storedCart) : [];
  
    const updatedCartItems = this.cartService.onCartUpdate();
  
    if (updatedCartItems && updatedCartItems.length > 0) {
      updatedCartItems.forEach((newItem: CartItem) => {
        const existingItem = cartData.find(
          (item: any) => item.productId === newItem.productId
        );
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
  
      cartData.forEach(
        (cartItem: { productId: string; cartQuantity: number }) => {
          this.productService
            .getProductById(cartItem.productId)
            .subscribe((product: any) => {
              this.cartItems.push({
                ...product,
                cartQuantity: cartItem.cartQuantity,
              });
            });
        }
      );
    }
  }
  
  addItem(Id: string) {
    const product = this.cartItems.find((item) => item.productId === Id);
    if (product) {
      const cartQuantity = product.cartQuantity;
      const maxQuantity = product.stockQuantity;
  
      if (maxQuantity <= 0) {
        alert('This product is out of stock.');
        return;
      }
  
      if (cartQuantity >= maxQuantity) {
        alert('No more stock available.');
        return;
      }
  
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
  
  grandTotal() {
    let grandTotal = 0.0;
  
    this.cartItems.forEach((element) => {
      grandTotal += element.cartQuantity * element.price;
    });
  
    return grandTotal;
  }
  
  makePurchase(productId: string, buyerId: string, quantity: number) {
    const purchase = new MakePurchase(productId, buyerId, quantity);
  
    this.cartService.onPurchase(purchase).subscribe(
      (res: any) => {
        console.log('Purchase successful', res);
      },
      (err: any) => {
        console.error('Purchase failed', err);
      }
    );
  }
  
  checkout() {
    const userId = sessionStorage.getItem('userId');
    if (!userId) {
      console.error('User ID is missing. Cannot proceed with checkout.');
      return;
    }
  
    if (!Array.isArray(this.cartItems) || this.cartItems.length === 0) {
      console.error('Cart is empty. No items to checkout.');
      return;
    }
  
    this.cartItems.forEach((item) => {
      if (!item.productId || !item.cartQuantity) {
        console.error('Invalid cart item:', item);
        return;
      }
  
      this.makePurchase(item.productId, userId, item.cartQuantity);
    });
  
    this.cartClear();
  }
  
  cartClear() {
    this.cartItems = [];
    sessionStorage.removeItem('cart');
  }
}
