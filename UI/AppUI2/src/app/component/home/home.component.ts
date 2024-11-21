import { Component, inject, OnInit } from '@angular/core';
import { Filter, Product } from '../../model/model';
import { HttpClient } from '@angular/common/http';
import { ProductService } from '../../services/product.service';
import { DatePipe } from '@angular/common';
import { IFilter } from '../../model/interface';

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
  productService = inject(ProductService)
  filter = new Filter();

  ngOnInit(): void {
    // this.getAllProducts()
    this.getSortedProducts();
  }

  getAllProducts(){
    this.productService.getAllProducts().subscribe((res: any)=>{
      this.products = res;
    })
  }

  getSortedProducts(){
    this.productService.getSortedProducts(this.filter).subscribe((res: any)=>{
      console.log(res);
      this.products = res;
    })
  }
  
}
