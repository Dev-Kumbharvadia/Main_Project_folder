import { CurrencyPipe, NgFor, NgIf } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { SalesData } from '../../model/model';
import { MiscService } from '../../services/misc.service';

@Component({
  selector: 'app-retailer-stats',
  standalone: true,
  imports: [CurrencyPipe, FormsModule, NgIf],
  templateUrl: './retailer-stats.component.html',
  styleUrl: './retailer-stats.component.css',
})
export class RetailerStatsComponent implements OnInit {
  showDetails: boolean = false;
  salesData: SalesData = new SalesData();
  miscServices = inject(MiscService);
  products: any;

  ngOnInit(): void {
    this.loadSalesData();
  }

  loadSalesData() {
    this.miscServices.loadSalesData().subscribe((res: any) => {
      this.products = res.data.itemsSold;
      this.salesData = res;
    });
  }

  toggleDetails(): void {
    this.showDetails = !this.showDetails;
  }
}
