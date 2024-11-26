import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environment/environment.development';
import { Constant } from '../constant/constant';
import { IFilter } from '../model/interface';
import { Product } from '../model/model';

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  constructor (private http: HttpClient) {

  }

  productsList: Product[] = [];
  updateProdId: string = '';

  getAllProducts(): Observable<any>{
    return this.http.get<any>(environment.API_URl + Constant.API_METHOD.PRODUCT.GET_ALL);
  }

  getSortedProductsBySellerId(filter: IFilter): Observable<any> {
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

    const url = `${environment.API_URl}${Constant.API_METHOD.PRODUCT.GET_SORTED_BY_SELLER_ID}?${params.toString()}`;

    return this.http.get<any>(url + `&SellerId=${sessionStorage.getItem('userId')}`);
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
    return this.http.get<any>(environment.API_URl + Constant.API_METHOD.PRODUCT.GET_BY_ID + `?id=${Id}`);
  }

  addProduct(formData: FormData): Observable<any>{
    return this.http.post<any>(environment.API_URl + Constant.API_METHOD.PRODUCT.ADD,formData);
  }

  updateProduct(productId: string, formData: FormData): Observable<any>{
    return this.http.put<any>(environment.API_URl + Constant.API_METHOD.PRODUCT.UPDATE+'?id='+`${productId}`,formData);
  }

  deleteProduct(Id: string): Observable<any>{
    return this.http.delete<any>(environment.API_URl + Constant.API_METHOD.PRODUCT.DELETE + "?id=" + `${Id}`,{});
  }

}
