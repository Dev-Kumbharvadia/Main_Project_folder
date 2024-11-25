import { CurrencyPipe, NgFor, NgIf } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-retailer-stats',
  standalone: true,
  imports: [CurrencyPipe, FormsModule, NgFor, NgIf],
  templateUrl: './retailer-stats.component.html',
  styleUrl: './retailer-stats.component.css'
})
export class RetailerStatsComponent {
  // Properties
  retailerName: string = 'John Doe';
  totalItemsSold: number = 150;
  totalMoneyEarned: number = 2500.00;
  showDetails: boolean = false;

  searchQuery: string = '';
  selectedFilter: string = 'all';

  // Mock product data
  products = [
    { productName: 'Product A', category: 'Electronics', price: 100, quantitySold: 5, totalEarned: 500 },
    { productName: 'Product B', category: 'Clothing', price: 50, quantitySold: 10, totalEarned: 500 },
    { productName: 'Product C', category: 'Books', price: 20, quantitySold: 20, totalEarned: 400 },
    { productName: 'Product D', category: 'Electronics', price: 200, quantitySold: 2, totalEarned: 400 }
  ];

  // Toggle the visibility of the details table
  toggleDetails(): void {
    this.showDetails = !this.showDetails;
  }

  // Filtered products based on search query and selected filter
  filteredProducts() {
    let filtered = this.products;

    // Filter by search query
    if (this.searchQuery) {
      filtered = filtered.filter(product =>
        product.productName.toLowerCase().includes(this.searchQuery.toLowerCase())
      );
    }

    // Apply additional filters
    if (this.selectedFilter === 'category') {
      filtered = filtered.filter(product => product.category === 'Electronics'); // Example condition
    } else if (this.selectedFilter === 'price') {
      filtered = filtered.sort((a, b) => a.price - b.price); // Example: Sort by price
    } else if (this.selectedFilter === 'date') {
      // Assuming a 'date' field exists in products (not in the example data)
      // filtered = filtered.sort((a, b) => new Date(a.date).getTime() - new Date(b.date).getTime());
    }

    return filtered;
  }
}