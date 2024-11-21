import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environment/environment.development';
import { Constant } from '../constant/constant';
import { IFilter } from '../model/interface';

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  constructor (private http: HttpClient) {

  }

  getAllProducts(): Observable<any>{
    return this.http.get<any>(environment.API_URl + Constant.API_METHOD.PRODUCT.GET_ALL);
  }

  getSortedProducts(filter: IFilter): Observable<any> {
    const params = new URLSearchParams();
  
    if (filter.Filters) {
      params.append('Filters', filter.Filters);
    }
  
    if (filter.Sorts) {
      params.append('Sorts', filter.Sorts);
    }
  
    if (filter.Page !== undefined) {
      params.append('Page', filter.Page.toString());
    }
  
    if (filter.PageSize !== undefined) {
      params.append('PageSize', filter.PageSize.toString());
    }
  
    const url = `${environment.API_URl}${Constant.API_METHOD.PRODUCT.GET_SORTED}?${params.toString()}`;
  
    return this.http.get<any>(url);
  }
  

  getProductById(Id: string): Observable<any>{
    return this.http.get<any>(environment.API_URl + Constant.API_METHOD.PRODUCT);
  }

  addProduct(product: any): Observable<any>{
    return this.http.post<any>(environment.API_URl + Constant.API_METHOD.PRODUCT,{});
  }

  updateProduct(product: any): Observable<any>{
    return this.http.put<any>(environment.API_URl + Constant.API_METHOD.PRODUCT,{});
  }

  deleteProduct(Id: string): Observable<any>{
    return this.http.delete<any>(environment.API_URl + Constant.API_METHOD.PRODUCT,{});
  }

}
